using Global.Abstractions.Global.Account;
using Global.Abstractions.Santa.Areas.Account;

namespace Application.Santa.Areas.Account.Commands;

public class ResetPasswordCommand<TItem> : ChangePasswordBaseCommand<TItem> where TItem : IResetPassword
{
    private readonly ISantaUser _user;

    public ResetPasswordCommand(TItem item, ISantaUser user) : base(item)
    {
        _user = user;
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
       Global_User? dbGlobalUser = GetGlobalUser(_user);

        if (dbGlobalUser != null && !string.IsNullOrWhiteSpace(Item.Password))
        {
            await ChangePassword(dbGlobalUser);
        }
        else
        {
            AddUserNotFoundError();
        }

        return await Result();
    }
}
