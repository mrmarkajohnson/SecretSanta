using Application.Areas.Messages.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using static Global.Settings.MessageSettings;

namespace Application.Areas.GiftingGroup.Commands;

public sealed class ReviewJoinerApplicationCommand<TItem> : GiftingGroupBaseCommand<TItem> where TItem : IReviewApplication
{
    private string _participateUrl;

    public ReviewJoinerApplicationCommand(TItem item, string participateUrl) : base(item)
    {
        _participateUrl = participateUrl;
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var dbApplication = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null && x.GroupAdmin)
            .Select(x => x.GiftingGroup)
            .SelectMany(x => x.MemberApplications)
            .FirstOrDefault(x => x.GroupApplicationKey == Item.GroupApplicationKey);

        if (dbApplication == null)
        {
            dbApplication = DbContext.Santa_GiftingGroupApplications.FirstOrDefault(x => x.GroupApplicationKey == Item.GroupApplicationKey);

            if (dbApplication != null && dbApplication.GiftingGroup.DateDeleted == null)
            {
                var dbLinks = dbCurrentSantaUser.GiftingGroupLinks
                    .Where(x => x.GiftingGroupKey == dbApplication.GiftingGroupKey && x.GroupAdmin)
                    .ToList();

                if (!dbLinks.Any() == true)
                {
                    throw new AccessDeniedException();
                }
            }

            throw new NotFoundException("Application");
        }

        if (Validation.IsValid && dbApplication != null)
        {
            dbApplication.Accepted = Item.Accepted;
            dbApplication.RejectionMessage = Item.Accepted ? null : Item.RejectionMessage;
            dbApplication.Blocked = Item.Accepted ? false : Item.Blocked;
            dbApplication.ResponseBySantaUserKey = dbCurrentSantaUser.SantaUserKey;

            if (Item.Accepted)
            {
                AddToGiftingGroup(dbApplication.GiftingGroup, dbApplication.SantaUser);
            }

            SendMessage(dbCurrentSantaUser, dbApplication);
            return await SaveAndReturnSuccess();
        }

        return await Result();
    }

    private void SendMessage(Santa_User dbCurrentSantaUser, Santa_GiftingGroupApplication dbApplication)
    {
        var message = new SendSantaMessage
        {
            RecipientType = Item.Accepted ? MessageRecipientType.SingleGroupMember : MessageRecipientType.SingleNonGroupMember,
            HeaderText = string.Empty, // set below
            MessageText = string.Empty, // ditto
            Important = false,
            CanReply = false,
            ShowAsFromSanta = true
        };

        if (Item.Accepted)
        {
            message.HeaderText = $"Welcome to group '{dbApplication.GiftingGroup.Name}'!";
            message.MessageText = $"Your application to join group '{dbApplication.GiftingGroup.Name}' has been accepted. " +
                $"Click {MessageLink(_participateUrl, "here", false)} to take part.";
        }
        else
        {
            message.HeaderText = $"Your application for '{dbApplication.GiftingGroup.Name}' was not accepted.";
            message.MessageText = $"Sorry, you haven't been accepted into group '{dbApplication.GiftingGroup.Name}'.";

            if (Item.RejectionMessage.IsNotEmpty())
            {
                message.MessageText += " " + Item.RejectionMessage;
            }

            if (Item.Blocked)
            {
                message.MessageText += " As this is not the first time, you are now blocked from applying again.";
            }
        }

        SendMessage(message, dbCurrentSantaUser, dbApplication.SantaUser, dbApplication.GiftingGroup);
    }
}
