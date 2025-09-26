using Application.Areas.GiftingGroup.Queries.Internal;
using Application.Areas.Messages.BaseModels;
using static Global.Settings.MessageSettings;

namespace Application.Areas.GiftingGroup.Commands;

public class AcceptInvitationCommand : GiftingGroupBaseCommand<string>
{
    private readonly string _participateUrl;
    private readonly bool _ignoreName; 
    
    public AcceptInvitationCommand(string invitationId, string participateUrl, bool ignoreName) : base(invitationId)
    {
        _participateUrl = participateUrl;
        _ignoreName = ignoreName;
    }

    protected async override Task<ICommandResult<string>> HandlePostValidation()
    {
        try
        {
            Santa_Invitation? dbInvitation = await Send(new GetInvitationEntityQuery(Item));

            if (dbInvitation == null)
                return await Result();

            if (!SignInManager.IsSignedIn(ClaimsUser))
                return await Result();

            Santa_User dbCurrentSantaUser = GetCurrentSantaUser();

            if (dbInvitation.ToSantaUserKey > 0)
            {
                if (dbCurrentSantaUser != null && dbCurrentSantaUser.SantaUserKey == dbInvitation.ToSantaUserKey) // otherwise it's for someone else
                {
                    return await AcceptInvitation(dbInvitation, dbCurrentSantaUser);
                }
            }
            else if (dbCurrentSantaUser.GlobalUser.Email.IsNotEmpty())
            {
                if (NameMustMatch(dbInvitation) && !NameMatches(dbInvitation.ToName, dbCurrentSantaUser))
                {
                    AddGeneralValidationError($"This invitation is to name {dbInvitation.ToName}. " +
                        $"Do you still wish to accept it?");
                }

                if (dbCurrentSantaUser.GlobalUser.Email == dbInvitation.ToEmailAddress)
                {
                    dbInvitation.ToSantaUser = dbCurrentSantaUser;
                    return await AcceptInvitation(dbInvitation, dbCurrentSantaUser);
                }
            }
        }
        catch
        {            
        }

        return await Result();
    }

    private bool NameMatches(string? toName, Santa_User dbSantaUser)
    {
        return string.Equals(toName, dbSantaUser.GlobalUser.Forename, StringComparison.InvariantCultureIgnoreCase)
            || string.Equals(toName, dbSantaUser.GlobalUser.PreferredFirstName, StringComparison.InvariantCultureIgnoreCase);
    }

    private bool NameMustMatch(Santa_Invitation dbInvitation)
    {
        return !_ignoreName && dbInvitation.ToName.IsNotEmpty();
    }

    private async Task<ICommandResult<string>> AcceptInvitation(Santa_Invitation dbInvitation, Santa_User dbSantaUser)
    {
        AddToGiftingGroup(dbInvitation.GiftingGroup, dbSantaUser);
        dbInvitation.DateArchived = DateTime.Now;
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
