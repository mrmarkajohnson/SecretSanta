using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Commands;

public sealed class ResetPasswordCommand<TItem> : ChangePasswordBaseCommand<TItem> where TItem : IResetPassword
{
    private readonly ISantaUser _user;

    public ResetPasswordCommand(TItem item, ISantaUser user) : base(item)
    {
        _user = user;
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Global_User? dbGlobalUser = GetGlobalUser(_user);

        if (dbGlobalUser != null && Item.Password.NotEmpty())
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
