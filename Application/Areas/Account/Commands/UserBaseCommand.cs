using Application.Areas.Messages.BaseModels;
using Application.Areas.Messages.ViewModels;
using Application.Shared.Requests;
using FluentValidation;
using Global.Abstractions.Areas.Account;
using Global.Extensions.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Application.Areas.Account.Commands;

public abstract class UserBaseCommand<TItem> : BaseCommand<TItem>
{
    public UserBaseCommand(TItem item) : base(item)
    {
    }

    protected async Task<bool> CheckPasswordAndHandleFailure(IConfirmCurrentPassword item, Global_User dbGlobalUser)
    {
        bool passwordCorrect = await UserManager.CheckPasswordAsync(dbGlobalUser, item.CurrentPassword);
        if (!passwordCorrect)
        {
            item.LockedOut = await AccessFailed(UserManager, dbGlobalUser);
            AddValidationError(nameof(item.CurrentPassword), "Current Password is incorrect.");
        }

        return passwordCorrect;
    }

    private protected IUserEmailStore<IdentityUser> GetEmailStore(IUserStore<IdentityUser> userStore)
    {
        if (!UserManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<IdentityUser>)userStore;
    }

    private protected async Task StoreEmailAddress(Global_User dbGlobalUser, string? hashedEmail,
        IUserStore<IdentityUser> userStore, string? unhashedEmail)
    {
        if (hashedEmail.IsNotEmpty())
        {
            try
            {
                await GetEmailStore(userStore).SetEmailAsync(dbGlobalUser, hashedEmail, CancellationToken.None);
            }
            catch
            {
                try
                {
                    await UserManager.SetEmailAsync(dbGlobalUser, hashedEmail);
                }
                catch
                {
                    string token = await UserManager.GenerateChangeEmailTokenAsync(dbGlobalUser, hashedEmail);
                    await UserManager.ChangeEmailAsync(dbGlobalUser, hashedEmail, token);
                }
            }

            unhashedEmail ??= EncryptionHelper.DecryptEmail(hashedEmail);
            SendEmailConfirmation(dbGlobalUser, unhashedEmail);
        }
        else
        {
            dbGlobalUser.Email = null;
        }

        dbGlobalUser.EmailConfirmed = false;
    }

    private protected string ReplaceHashedDetails(string message, string? originalUserName, string? originalEmail)
    {
        if (Item is IHashableUserBase hasUserName && hasUserName.UserName.IsNotEmpty())
        {
            message = message.Replace(hasUserName.UserName, originalUserName);
        }

        if (Item is IHasEmail iHasEmail && iHasEmail.Email.IsNotEmpty())
        {
            message = message.Replace(iHasEmail.Email, originalEmail);
        }

        return message;
    }

    protected void SendEmailConfirmation(Global_User dbGlobalUser, string unhashedEmail)
    {
        if (DbContext.EmailClient == null)
            throw new NotFoundException("The e-mail client has not been configured.");

        if (string.IsNullOrWhiteSpace(MessageSettings.ConfirmEmailUrl))
            throw new NotFoundException("The e-mail confirmation URL has not been set.");

        string confirmationId = EncryptionHelper.GetEmaiConfirmationId(unhashedEmail, dbGlobalUser);
        string? confirmUrl = $"{MessageSettings.ConfirmEmailUrl}?id={confirmationId}";
        string messageText = $"Please {MessageLink(confirmUrl, "click here", false)} to confirm your e-mail address.";

        var message = new SantaMessage
        {
            HeaderText = "Please confirm your e-mail address",
            MessageText = messageText,
            ShowAsFromSanta = true
        };

        var recipient = new EmailRecipient
        {
            Forename = dbGlobalUser.PreferredFirstName ?? dbGlobalUser.Forename,
            Surname = dbGlobalUser.Surname,
            Email = unhashedEmail,
            IdentificationHashed = false,
            EmailConfirmed = true,
            ReceiveEmails = MessageSettings.EmailPreference.All,
            DetailedEmails = true
        };

        try
        {
            Validation.AddResult(DbContext.EmailClient.SendMessage(message, [recipient]));
        }
        catch (Exception ex)
        {
            Validation.AddError(ex.Message);
        }
    }
}
