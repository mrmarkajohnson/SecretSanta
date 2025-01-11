using Application.Shared.Requests;
using Global.Abstractions.Areas.Account;

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
}
