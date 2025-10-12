using Application.Areas.GiftingGroup.Queries.Internal;
using Application.Areas.Messages.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using static Global.Settings.MessageSettings;

namespace Application.Areas.GiftingGroup.Commands;

public class ReviewInvitationCommand : GiftingGroupBaseCommand<IReviewGroupInvitation>
{
    private readonly string _participateUrl;

    public ReviewInvitationCommand(IReviewGroupInvitation item, string participateUrl) : base(item)
    {
        _participateUrl = participateUrl;
    }

    protected async override Task<ICommandResult<IReviewGroupInvitation>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser();
        Santa_Invitation? dbInvitation = null;

        try
        {
            await Send(new GetInvitationEntityQuery(Item.InvitationGuid));
        }
        catch (Exception ex)
        {
            return await ReturnGeneralError(ex.Message);
        }

        if (dbInvitation == null)
        {
            return await ReturnGeneralError("This invitation could not be found.");
        }

        if (dbInvitation.ToSantaUserKey == null)
        {
            dbInvitation.ToSantaUserKey = dbCurrentSantaUser.SantaUserKey;
            dbInvitation.ToSantaUser = dbCurrentSantaUser;
        }
        else if (dbInvitation.ToSantaUserKey != dbCurrentSantaUser.SantaUserKey) // this should have been caught already, but just in case
        {
            return await ReturnGeneralError("This invitation is for a different user.");
        }

        if (Item.Accept == true)
        {
            AcceptInvitation(dbInvitation, dbCurrentSantaUser);
            return await SaveAndReturnSuccess($"You've now joined group '{dbInvitation.GiftingGroup.Name}'.");
        }
        else if (Item.Accept == false)
        {
            RejectInvitation(dbInvitation, dbCurrentSantaUser);
            return await SaveAndReturnSuccess(); // save the ToSantaUserKey
        }
        else
        {
            return await SaveAndReturnSuccess(); // save the ToSantaUserKey
        }
    }

    private void RejectInvitation(Santa_Invitation dbInvitation, Santa_User dbSantaUser)
    {
        dbInvitation.DateArchived = DateTime.Now;
        SendRejectedMessage(dbInvitation, dbSantaUser);
    }

    private void SendRejectedMessage(Santa_Invitation dbInvitation, Santa_User dbSantaUser)
    {
        if (dbInvitation.ToSantaUser == null)
            return; // for the compiler

        string messageText = $"{dbInvitation.ToSantaUser.GlobalUser.DisplayName()} rejected your invitation " +
                $"to join group '{dbInvitation.GiftingGroup.Name}'.";

        if (Item.RejectionMessage.IsNotEmpty())
        {
            messageText += $"<br><br>{dbSantaUser.GlobalUser.Gender.Direct(true)} said: \"{dbInvitation.Message.Trim()}\"";
        }

        var message = new SendSantaMessage
        {
            RecipientType = MessageRecipientType.SingleGroupMember,
            HeaderText = $"{dbInvitation.ToSantaUser.GlobalUser.DisplayName()} has " +
                $"not joined '{dbInvitation.GiftingGroup.Name}'",
            MessageText = messageText,
            Important = false,
            CanReply = false,
            ShowAsFromSanta = true
        };

        SendMessage(message, dbInvitation.ToSantaUser, dbInvitation.FromSantaUser, dbInvitation.GiftingGroup);
    }

    private void AcceptInvitation(Santa_Invitation dbInvitation, Santa_User dbSantaUser)
    {
        AddToGiftingGroup(dbInvitation.GiftingGroup, dbSantaUser);
        dbInvitation.DateArchived = DateTime.Now;
        SendWelcomeMessage(dbInvitation);
        SendAcceptedMessage(dbInvitation);
    }

    private void SendWelcomeMessage(Santa_Invitation dbInvitation)
    {
        if (dbInvitation.ToSantaUser == null)
            return; // for the compiler

        var message = new SendSantaMessage
        {
            RecipientType = MessageRecipientType.SingleGroupMember,
            HeaderText = $"Welcome to group '{dbInvitation.GiftingGroup.Name}'!",
            MessageText = $"Click {MessageLink(_participateUrl, "here", false)} to take part.",
            Important = false,
            CanReply = false,
            ShowAsFromSanta = true
        };

        SendMessage(message, dbInvitation.FromSantaUser, dbInvitation.ToSantaUser, dbInvitation.GiftingGroup);
    }

    private void SendAcceptedMessage(Santa_Invitation dbInvitation)
    {
        if (dbInvitation.ToSantaUser == null)
            return; // for the compiler

        var message = new SendSantaMessage
        {
            RecipientType = MessageRecipientType.SingleGroupMember,
            HeaderText = $"{dbInvitation.ToSantaUser.GlobalUser.DisplayName()} has joined '{dbInvitation.GiftingGroup.Name}'",
            MessageText = $"{dbInvitation.ToSantaUser.GlobalUser.DisplayName()} has accepted your invitation " +
                $"to join group '{dbInvitation.GiftingGroup.Name}'",
            Important = false,
            CanReply = false,
            ShowAsFromSanta = true
        };

        SendMessage(message, dbInvitation.ToSantaUser, dbInvitation.FromSantaUser, dbInvitation.GiftingGroup);
    }
}
