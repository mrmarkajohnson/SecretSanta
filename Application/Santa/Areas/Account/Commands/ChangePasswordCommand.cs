using FluentValidation.Results;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public class ChangePasswordCommand<TItem> : BaseCommand<TItem> where TItem : IChangePassword
{
    private readonly ISantaUser _user;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public ChangePasswordCommand(TItem item,
        ISantaUser user,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : base(item)
    {
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
        var globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.Id == _user.Id);

        if (globalUserDb != null && !string.IsNullOrWhiteSpace(Item.Password))
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(globalUserDb); // can't call the reset directly

            var resetUser = await _userManager.FindByIdAsync(globalUserDb.Id); // avoid 'cannot be tracked' error
            if (resetUser != null)
            {
                var result = await _userManager.ResetPasswordAsync(resetUser, token, Item.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(globalUserDb, isPersistent: false);
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
        }
        else
        {
            AddUserNotFoundError();
        }

        return await Result();
    }
}
