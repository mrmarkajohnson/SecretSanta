using Application.Areas.GiftingGroup.Commands;
using Application.Areas.Messages.Queries.Internal;
using Global.Abstractions.Areas.Messages;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.Commands;

public sealed class WriteMessageCommand<TItem> : GiftingGroupYearBaseCommand<TItem> where TItem : IWriteSantaMessage
{
    public WriteMessageCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Item.CanReply = true;

        if (Item.RecipientType == MessageRecipientType.SystemAdmins)
        {
            return await ReportIssue();
        }
        else
        {
            return await SendNormalMessage();
        }
    }

    private async Task<ICommandResult<TItem>> ReportIssue()
    {
        Item.IncludeFutureMembers = true;
        Santa_User dbCurrentUser = GetCurrentSantaUser();
        IList<Santa_User> dbRecipients = await Send(new GetPossibleMessageRecipientsQuery(dbCurrentUser, null, Item));

        SendMessage(Item, dbCurrentUser, dbRecipients);
        return await SaveAndReturnSuccess();
    }

    private async Task<ICommandResult<TItem>> SendNormalMessage()
    {
        if (Item.GiftingGroupKey == null) // just in case
        {
            AddValidationError(nameof(Item.GiftingGroupKey), ValidationMessages.RequiredError);
            return await Result();
        }

        Santa_User dbCurrentUser = GetCurrentSantaUser();

        Santa_GiftingGroup dbGiftingGroup = await GetGiftingGroup(Item.GiftingGroupKey.Value, false);
        Santa_GiftingGroupYear dbGiftingGroupYear = GetOrCreateGiftingGroupYear(dbGiftingGroup);

        IList<Santa_User> dbRecipients = await Send(new GetPossibleMessageRecipientsQuery(dbCurrentUser, dbGiftingGroupYear, Item));

        if (dbRecipients.Count == 0 && !Item.IncludeFutureMembers)
        {
            string futureLabel = Item.RecipientType.FutureLabel();

            if (futureLabel.IsNotEmpty())
            {
                string othersDescription = Item.RecipientType.SenderToDescription(dbGiftingGroup.Name).Replace("All ", "").Replace("other ", "");

                AddValidationError(nameof(Item.IncludeFutureMembers),
                    $"There are currently no other {othersDescription}. Please select '{futureLabel}' to ensure your message can be read.");
            }
        }

        if (!Validation.IsValid)
            return await Result();

        Item.SetActualRecipientType();

        Item.ShowAsFromSanta = Item.RecipientType is MessageRecipientType.GiftRecipient
            or MessageRecipientType.PotentialPartner or MessageRecipientType.SingleNonGroupMember;

        var dbMessage = SendMessage(Item, dbCurrentUser, dbRecipients, dbGiftingGroupYear);
        await HandleReply(dbCurrentUser, dbMessage);

        return await SaveAndReturnSuccess();
    }

    private async Task HandleReply(Santa_User dbCurrentUser, Santa_Message dbMessage)
    {
        if (Item.ReplyToMessageKey > 0)
        {
            Santa_Message dbReplyToMessage = await Send(new GetOriginalMessageQuery(Item.ReplyToMessageKey.Value, dbCurrentUser, false));
            dbMessage.ReplyToMessage = dbReplyToMessage;
            dbMessage.ReplyToMessageKey = dbReplyToMessage.MessageKey;

            if (dbReplyToMessage.SenderKey == dbCurrentUser.SantaUserKey) // only set the original message when replying to sent messaes, as someone may have replied to a group message who wan't the first sender
            {
                dbMessage.OriginalMessageKey = dbReplyToMessage.OriginalMessageKey ?? dbReplyToMessage.ReplyToMessageKey;
            }

            Santa_Message dbFirstMessage = dbReplyToMessage.OriginalMessage ?? dbReplyToMessage.ReplyToMessage ?? dbReplyToMessage;

            if (dbFirstMessage.RecipientType is MessageRecipientType.GiftRecipient or MessageRecipientType.Gifter)
            {
                bool firstMessageWasSentMessage = dbFirstMessage.SenderKey == dbCurrentUser.SantaUserKey;

                if ((dbFirstMessage.RecipientType == MessageRecipientType.GiftRecipient) != firstMessageWasSentMessage) // first message was from the recipient, or to the giver...
                {
                    dbMessage.RecipientType = Item.RecipientType = MessageRecipientType.Gifter; // ... so this must be to the giver (can't reply to yourself)
                }
                else
                {
                    dbMessage.RecipientType = Item.RecipientType = MessageRecipientType.GiftRecipient;
                    dbMessage.ShowAsFromSanta = Item.ShowAsFromSanta = true;
                }
            }
        }
    }
}