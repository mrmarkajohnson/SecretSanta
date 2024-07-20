using Data.Entities.Shared;
using Global.Abstractions.Global.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public abstract class ChangePasswordBaseCommand<TItem> : UserBaseCommand<TItem> where TItem : ISetPassword
{
    protected ChangePasswordBaseCommand(TItem item, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) 
        : base(item, userManager, signInManager)
    {
    }

    protected async Task ChangePassword(Global_User globalUserDb)
    {
        string token = await UserManager.GeneratePasswordResetTokenAsync(globalUserDb); // can't call the reset directly

        var resetUser = await UserManager.FindByIdAsync(globalUserDb.Id); // avoid 'cannot be tracked' error
        if (resetUser != null)
        {
            var result = await UserManager.ResetPasswordAsync(resetUser, token, Item.Password);

            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(globalUserDb, isPersistent: false);
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
