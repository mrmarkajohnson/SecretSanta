using Application.Areas.GiftingGroup.Commands;
using Application.Areas.Messages.Queries.Internals;
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
        if (Item.GiftingGroupKey == null) // just in case
        {
            AddValidationError(nameof(Item.GiftingGroupKey), ValidationMessages.RequiredError);
            return await Result();
        }

        Santa_User dbCurrentUser = GetCurrentSantaUser();

        Item.CanReply = true;

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

        Item.ShowAsFromSanta = Item.RecipientType is MessageRecipientType.Gifter;

        if (!Validation.IsValid)
            return await Result();

        Item.SetActualRecipientType();
        Item.ShowAsFromSanta = Item.RecipientType is MessageRecipientType.GiftRecipient or MessageRecipientType.PotentialPartner;
        
        var dbMessage = SendMessage(Item, dbCurrentUser, dbRecipients, dbGiftingGroupYear);

        if (Item.ReplyToMessageKey > 0)
        {
            Santa_Message dbOriginalMessage = await Send(new GetOriginalMessageQuery(Item.ReplyToMessageKey.Value, dbCurrentUser));
            dbMessage.ReplyToMessage = dbOriginalMessage;
            dbMessage.ReplyToMessageKey = dbOriginalMessage.MessageKey;

            if (dbOriginalMessage.SenderKey == dbCurrentUser.SantaUserKey)
            {
                dbMessage.OriginalMessageKey = dbOriginalMessage.OriginalMessageKey ?? dbMessage.ReplyToMessageKey;
            }
        }

        return await SaveAndReturnSuccess();
    }
}