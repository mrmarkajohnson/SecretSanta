using Global.Abstractions.Global.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public abstract class ChangePasswordBaseCommand<TItem> : UserBaseCommand<TItem> where TItem : ISetPassword
{
    protected ChangePasswordBaseCommand(TItem item, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) 
        : base(item, userManager, signInManager)
    {
    }

    protected async Task ChangePassword(Global_User dbGlobalUser)
    {
        string token = await UserManager.GeneratePasswordResetTokenAsync(dbGlobalUser); // can't call the reset directly

        var resetUser = await UserManager.FindByIdAsync(dbGlobalUser.Id); // avoid 'cannot be tracked' error
        if (resetUser != null)
        {
            var result = await UserManager.ResetPasswordAsync(resetUser, token, Item.Password);

            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(dbGlobalUser, isPersistent: false);
                Success = true;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    AddValidationError(nameof(Item.Password), error.Description);
                }
            }
        }
        else
        {
            AddUserNotFoundError();
        }
    }

}
