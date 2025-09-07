namespace Application.Areas.Account.Commands;

public class SendEmailConfirmationCommand : UserBaseCommand<bool>
{
    public SendEmailConfirmationCommand() : base(false)
    {
    }

    protected async override Task<ICommandResult<bool>> HandlePostValidation()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser();

        if (string.IsNullOrWhiteSpace(dbCurrentUser.Email))
            throw new ArgumentException($"Cannot confirm {UserDisplayNames.Email}, as it is empty.");

        string unhashedEmail = EncryptionHelper.DecryptEmail(dbCurrentUser.Email);
        SendEmailConfirmation(dbCurrentUser, unhashedEmail);

        Item = true;
        return await SuccessResult();
    }
}
