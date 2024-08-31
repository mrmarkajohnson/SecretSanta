using Global.Abstractions.Global.Account;

namespace Application.Santa.Areas.Account.Commands;

public class ChangePasswordCommand<TItem> : ChangePasswordBaseCommand<TItem> where TItem : IChangePassword
{
    public ChangePasswordCommand(TItem item) : base(item)
    {
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
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
            Global_User? dbGlobalUser = GetGlobalUser(userId);

            if (dbGlobalUser != null)
            {
                bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, dbGlobalUser);
                if (!passwordCorrect || !Validation.IsValid)
                {
                    return await Result();
                }
                else
                {
                    await ChangePassword(dbGlobalUser);
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
