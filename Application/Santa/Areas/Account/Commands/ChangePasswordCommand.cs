using FluentValidation.Results;
using Global.Abstractions.Global.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Commands;

public class ChangePasswordCommand<TItem> : ChangePasswordBaseCommand<TItem> where TItem : IChangePassword
{
    private readonly ClaimsPrincipal _user;

    public ChangePasswordCommand(TItem item,
        ClaimsPrincipal user,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : base(item, userManager, signInManager)
    {
        _user = user;
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
        if (SignInManager.IsSignedIn(_user))
        {
            string? userId = UserManager.GetUserId(_user);
            if (userId != null)
            {
                var globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.Id == userId);

                if (globalUserDb != null)
                {
                    bool passwordCorrect = await UserManager.CheckPasswordAsync(globalUserDb, Item.CurrentPassword);
                    if (!passwordCorrect)
                    {
                        Validation.Errors.Add(new ValidationFailure(nameof(Item.CurrentPassword), $"{Item.CurrentPasswordLabel} is incorrect."));
                    }
                    else if (Validation.IsValid && !string.IsNullOrWhiteSpace(Item.Password))
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
                                    Validation.Errors.Add(new ValidationFailure(nameof(Item.Password), error.Description));
                                }
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
        }

        return await Result();
    }
}
