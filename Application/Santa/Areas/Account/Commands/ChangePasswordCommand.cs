using Application.Santa.Global;
using FluentValidation.Results;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public class ChangePasswordCommand : BaseCommand<ISantaUser>
{
    private readonly IChangePassword _item;
    private readonly ISantaUser _user;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public ChangePasswordCommand(IChangePassword item,
        ISantaUser user,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : base(user)
    {
        _item = item;
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public override async Task<ICommandResult<ISantaUser>> Handle()
    {
        var globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.Id == _user.Id);

        if (globalUserDb != null && !string.IsNullOrWhiteSpace(_item.Password))
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(globalUserDb); // can't call the reset directly

            var resetUser = await _userManager.FindByIdAsync(globalUserDb.Id); // avoid 'cannot be tracked' error
            if (resetUser != null)
            {
                var result = await _userManager.ResetPasswordAsync(resetUser, token, _item.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(globalUserDb, isPersistent: false);
                    Success = true;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Validation.Errors.Add(new ValidationFailure(nameof(_item.Password), error.Description));
                    }
                }
            }
        }
        else
        {
            Validation.Errors.Add(new ValidationFailure(string.Empty, "User not found. Please log in again."));
        }

        return await Result();
    }
}
