using Data.Entities.Shared;
using Global.Abstractions.Global.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public abstract class UserBaseCommand<TItem> : BaseCommand<TItem>
{
    private protected UserManager<IdentityUser> UserManager { get; set; }
    private protected SignInManager<IdentityUser> SignInManager;

    public UserBaseCommand(TItem item,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : base(item)
    {
        UserManager = userManager;
        SignInManager = signInManager;
    }

    protected async Task<bool> CheckPasswordAndHandleFailure(IConfirmCurrentPassword item, Global_User globalUserDb)
    {
        bool passwordCorrect = await UserManager.CheckPasswordAsync(globalUserDb, item.CurrentPassword);
        if (!passwordCorrect)
        {
            item.LockedOut = await AccessFailed(UserManager, globalUserDb);
            AddValidationError(nameof(item.CurrentPassword), "Current Password is incorrect.");
        }

        return passwordCorrect;
    }
}
