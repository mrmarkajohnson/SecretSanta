using Application.Shared.Requests;

namespace Application.Areas.Account.Commands;

public class ConfirmEmailCommand : BaseCommand<string>
{
    public ConfirmEmailCommand(string item) : base(item)
    {
    }

    protected async override Task<ICommandResult<string>> HandlePostValidation()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser();

        if (string.IsNullOrWhiteSpace(dbCurrentUser.Email))
            throw new ArgumentException($"Cannot confirm {UserDisplayNames.Email}, as it is empty.");

        string unhashedEmail = EncryptionHelper.DecryptEmail(dbCurrentUser.Email);
        string expectedConfirmationId = EncryptionHelper.GetEmailConfirmationId(unhashedEmail, dbCurrentUser);

        if (Item != expectedConfirmationId)
            throw new ArgumentException($"Cannot confirm {UserDisplayNames.Email}, as the key does not match. Please check the URL in the confirmation e-mail.");

        dbCurrentUser.EmailConfirmed = true;
        return await SaveAndReturnSuccess();
    }
}
