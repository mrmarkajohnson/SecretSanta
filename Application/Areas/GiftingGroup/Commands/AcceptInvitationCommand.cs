using Application.Areas.GiftingGroup.Queries.Internal;
using Application.Areas.Messages.BaseModels;
using static Global.Settings.MessageSettings;

namespace Application.Areas.GiftingGroup.Commands;

public class AcceptInvitationCommand : GiftingGroupBaseCommand<string>
{
    public AcceptInvitationCommand(string invitationId, string participateUrl) : base(invitationId)
    {
        _participateUrl = participateUrl;
    }

    private readonly string _participateUrl;

    protected async override Task<ICommandResult<string>> HandlePostValidation()
    {
        try
        {
            Santa_Invitation? dbInvitation = await Send(new GetInvitationEntityQuery(Item));

            if (dbInvitation == null)
                return await Result();

            if (!SignInManager.IsSignedIn(ClaimsUser))
                return await Result();

            Santa_User dbSantaUser = GetCurrentSantaUser();

            if (dbInvitation.ToSantaUserKey > 0)
            {
                if (dbSantaUser != null && dbSantaUser.SantaUserKey == dbInvitation.ToSantaUserKey) // otherwise it's for someone else
                {
                    return await AcceptInvitation(dbInvitation, dbSantaUser);
                }
            }
            else if (dbSantaUser.GlobalUser.Email.IsNotEmpty())
            {
                if (dbSantaUser.GlobalUser.Email == dbInvitation.ToEmailAddress)
                {
                    return await AcceptInvitation(dbInvitation, dbSantaUser);
                }
            }
        }
        catch
        {            
        }

        return await Result();
    }

    private async Task<ICommandResult<string>> AcceptInvitation(Santa_Invitation dbInvitation, Santa_User dbSantaUser)
    {
        AddToGiftingGroup(dbInvitation.GiftingGroup, dbSantaUser);
        SendWelcomMessage(dbInvitation);
        SendAcceptedMessage(dbInvitation);

        return await SaveAndReturnSuccess($"You've now joined group '{dbInvitation.GiftingGroup.Name}'.");
    }

    private void SendWelcomMessage(Santa_Invitation dbInvitation)
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
            HeaderText = $"{dbInvitation.ToSantaUser.GlobalUser.DisplayName()} has " +
                $"joined '{dbInvitation.GiftingGroup.Name}'.",
            MessageText = $"{dbInvitation.ToSantaUser.GlobalUser.DisplayName()} has accepted your invitation " +
                $"to join group '{dbInvitation.GiftingGroup.Name}'.",
            Important = false,
            CanReply = false,
            ShowAsFromSanta = true
        };

        SendMessage(message, dbInvitation.ToSantaUser, dbInvitation.FromSantaUser, dbInvitation.GiftingGroup);
    }
}
