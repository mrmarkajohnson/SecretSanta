using Application.Areas.Messages.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using static Global.Settings.MessageSettings;

namespace Application.Areas.GiftingGroup.Commands;

public class SendInvitationCommand<TItem> : GiftingGroupBaseCommand<TItem> where TItem : ISendGroupInvitation
{
    public SendInvitationCommand(TItem item, string acceptUrl) : base(item)
    {
        _acceptUrl = acceptUrl;
    }

    private readonly string _acceptUrl;

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        Santa_GiftingGroupUser? dbGiftingGroupLink = dbCurrentSantaUser.GiftingGroupLinks
            .FirstOrDefault(x => x.GiftingGroupKey == Item.GiftingGroupKey);

        if (dbGiftingGroupLink == null || !dbGiftingGroupLink.GroupAdmin)
            throw new AccessDeniedException();

        if (Item.ToHashedUserId.IsNotEmpty())
        {
            return await ProcessSelectedUser(dbCurrentSantaUser, dbGiftingGroupLink);
        }
        else if (Item.ToEmailAddress.IsNotEmpty() && Item.ToName.IsNotEmpty())
        {
            return await ProcessEmailAddress(dbCurrentSantaUser, dbGiftingGroupLink);
        }
        else
        {
            AddGeneralValidationError($"Please either select a user, or enter a {UserDisplayNames.Forename} and {UserDisplayNames.EmailLower}.");
        }

        return await Result();
    }

    private async Task<ICommandResult<TItem>> ProcessSelectedUser(Santa_User dbCurrentSantaUser, Santa_GiftingGroupUser dbGiftingGroupLink)
    {
        var unhashedUserId = EncryptionHelper.Decrypt(Item.ToHashedUserId);

        Santa_User? dbToSantaUser = dbCurrentSantaUser.GiftingGroupLinks
            .Select(x => x.GiftingGroup)
            .Where(x => x.GiftingGroupKey != Item.GiftingGroupKey)
            .SelectMany(x => x.Members)
            .Where(y => y.SantaUserKey != dbCurrentSantaUser.SantaUserKey)
            .Select(y => y.SantaUser)
            .FirstOrDefault(y => y.GlobalUser.Id == unhashedUserId);

        if (dbToSantaUser == null)
        {
            AddGeneralValidationError("No matching user found.");
            return await Result();
        }
        else
        {
            return await SendUserInvitation(dbGiftingGroupLink, dbToSantaUser);
        }
    }

    private async Task<ICommandResult<TItem>> ProcessEmailAddress(Santa_User dbCurrentSantaUser, Santa_GiftingGroupUser dbGiftingGroupLink)
    {
        if (Item.ToEmailAddress == null || Item.ToName == null)
            return await Result();

        string hashedEmail = EncryptionHelper.EncryptEmail(Item.ToEmailAddress.Tidy());
        string tidiedName = Item.ToName.Tidy();

        IQueryable<Global_User> dbPossibleToUsers = DbContext.Global_Users
            .Where(x => x.SantaUser != null && x.SantaUser.SantaUserKey != dbCurrentSantaUser.SantaUserKey)
            .Where(x => x.Email == hashedEmail);

        Santa_User? dbToSantaUser = dbPossibleToUsers
            .Where(x => x.Forename == tidiedName || x.PreferredFirstName == tidiedName)
            .Select(x => x.SantaUser)
            .FirstOrDefault();

        if (dbToSantaUser == null)
        {
            bool emailExists = dbPossibleToUsers.Any();

            if (emailExists)
            {
                AddGeneralValidationError("A user was found matching the e-mail address entered, but their name did not match.");
                return await Result();
            }
            else
            {
                return await SendEmailInvitation(dbGiftingGroupLink, tidiedName);
            }
        }
        else
        {
            return await SendUserInvitation(dbGiftingGroupLink, dbToSantaUser);
        }
    }

    private async Task<ICommandResult<TItem>> SendUserInvitation(Santa_GiftingGroupUser dbGiftingGroupLink, Santa_User dbToSantaUser)
    {
        var dbInvitation = new Santa_Invitation
        {
            FromSantaUserKey = dbGiftingGroupLink.SantaUserKey,
            FromSantaUser = dbGiftingGroupLink.SantaUser,
            ToSantaUserKey = dbToSantaUser.SantaUserKey,
            ToSantaUser = dbToSantaUser,
            GiftingGroupKey = Item.GiftingGroupKey,
            GiftingGroup = dbGiftingGroupLink.GiftingGroup
        };

        return await SaveAndSendInvitation(dbInvitation);
    }

    private async Task<ICommandResult<TItem>> SendEmailInvitation(Santa_GiftingGroupUser dbGiftingGroupLink, string tidiedName)
    {
        var dbInvitation = new Santa_Invitation
        {
            FromSantaUserKey = dbGiftingGroupLink.SantaUserKey,
            FromSantaUser = dbGiftingGroupLink.SantaUser,
            ToName = tidiedName,
            ToEmailAddress = Item.ToEmailAddress,
            GiftingGroupKey = Item.GiftingGroupKey,
            GiftingGroup = dbGiftingGroupLink.GiftingGroup
        };

        return await SaveAndSendInvitation(dbInvitation);
    }

    private async Task<ICommandResult<TItem>> SaveAndSendInvitation(Santa_Invitation dbInvitation)
    {
        DbContext.Santa_Invitations.Add(dbInvitation);
        string acceptUrl = $"{_acceptUrl}?id={dbInvitation.GetInvitationId()}";

        if (dbInvitation.ToSantaUser != null)
        {
            SendToSantaUser(dbInvitation, acceptUrl);
            return await SaveAndReturnSuccess();
        }
        else
        {
            return await SendToEmail(dbInvitation, acceptUrl);
        }      
    }

    private void SendToSantaUser(Santa_Invitation dbInvitation, string acceptUrl)
    {
        if (dbInvitation.ToSantaUser == null)
            throw new ArgumentException("A user must be selected."); // for the compiler

        string messageText = $"{dbInvitation.FromSantaUser.GlobalUser.DisplayName()} has invited you to join the group '{dbInvitation.GiftingGroup.Name}'." +
            $"<br><br>Click {MessageLink(acceptUrl, "here", false)} to accept the invitation.";

        var message = new SendSantaMessage
        {
            RecipientType = MessageRecipientType.SingleNonGroupMember,
            HeaderText = $"You have been invited to join '{dbInvitation.GiftingGroup.Name}'",
            MessageText = messageText,
            Important = true,
            CanReply = true,
            ShowAsFromSanta = false
        };

        SendMessage(message, dbInvitation.FromSantaUser, dbInvitation.ToSantaUser, dbInvitation.GiftingGroup);
    }

    private async Task<ICommandResult<TItem>> SendToEmail(Santa_Invitation dbInvitation, string acceptUrl)
    {
        if (DbContext.EmailClient == null || dbInvitation.ToEmailAddress == null || dbInvitation.ToName == null)
        {
            AddGeneralValidationError("The invitation cannot be sent, due to an issue with e-mails.");
            return await Result();
        }

        var result = await SaveAndReturnSuccess();

        if (result.Success)
        {
            string baseUrl = ConfigurationSettings.BaseUrl ?? "";

            string messageText = $"{dbInvitation.FromSantaUser.GlobalUser.DisplayName()} has invited you to join the group '{dbInvitation.GiftingGroup.Name}'." +
                $"<br><br>Click {MessageLink(acceptUrl, "here", false, true)} to accept the invitation." +
                $"<br><br>If you don't already use Secret Santa, click {MessageLink(baseUrl, "here", false, true)} to find out more.";

            var message = new SantaMessage
            {
                RecipientType = MessageRecipientType.SingleNonGroupMember,
                HeaderText = $"You have been invited to join '{dbInvitation.GiftingGroup.Name}'",
                MessageText = messageText,
                Important = true,
                CanReply = true,
                ShowAsFromSanta = false
            };

            var recipient = new EmailRecipient
            {
                Forename = dbInvitation.ToName.Tidy(),
                Email = dbInvitation.ToEmailAddress.Tidy(),
                IdentificationHashed = false,
                EmailConfirmed = true,
                ReceiveEmails = EmailPreference.All,
                DetailedEmails = true
            };
            
            DbContext.EmailClient.SendMessage(message, [recipient]);
        }

        return result;
    }
}
