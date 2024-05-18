using Application.Santa.Global;
using FluentValidation.Results;
using Global.Abstractions.Global;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        var globalUserDb = ModelContext.Global_Users
                .FirstOrDefault(x => x.Id == _user.Id);

        if (globalUserDb != null && !string.IsNullOrWhiteSpace(_item.Password))
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(globalUserDb);
            var resetUser = await _userManager.FindByNameAsync(_user.UserName);
            var result = await _userManager.ResetPasswordAsync(resetUser, token, _item.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(globalUserDb, isPersistent: false);                
                Success = true;
            }
        }
        else
        {
            Validation.Errors.Add(new ValidationFailure(string.Empty, "User not found. Please log in again."));
        }

        return await Result();
    }
}
