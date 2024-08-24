using Global.Abstractions.Global.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public abstract class UserBaseCommand<TItem> : BaseCommand<TItem>
{
    private protected UserManager<IdentityUser> UserManager { get; set; }
    private protected SignInManager<IdentityUser> SignInManager { get; set; }

    public UserBaseCommand(TItem item,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : base(item)
    {
        UserManager = userManager;
        SignInManager = signInManager;
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
