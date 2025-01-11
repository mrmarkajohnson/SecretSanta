using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Commands;

public class ChangePasswordCommand<TItem> : ChangePasswordBaseCommand<TItem> where TItem : IChangePassword
{
    public ChangePasswordCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        if (!Validation.IsValid)
        {
            return await Result();
        }
        else if (string.IsNullOrWhiteSpace(Item.Password)) // just in case
        {
            AddValidationError(nameof(Item.Password), "Please enter a new password.");
            return await Result();
        }

        EnsureSignedIn();

        string? userId = GetCurrentUserId();
        if (userId != null)
        {
            Global_User? dbCurrentUser = GetGlobalUser(userId);

            if (dbCurrentUser != null)
            {
                bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, dbCurrentUser);
                if (!passwordCorrect || !Validation.IsValid)
                {
                    return await Result();
                }
                else
                {
                    await ChangePassword(dbCurrentUser);
                }
            }
            else
            {
                AddUserNotFoundError();
            }
        }
        else
        {
            AddUserNotFoundError();
        }

        return await Result();
    }
}
