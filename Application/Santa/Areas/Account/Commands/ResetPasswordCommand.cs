using Global.Abstractions.Global.Account;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public class ResetPasswordCommand<TItem> : ChangePasswordBaseCommand<TItem> where TItem : IResetPassword
{
    private readonly ISantaUser _user;

    public ResetPasswordCommand(TItem item,
        ISantaUser user,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : base(item, userManager, signInManager)
    {
        _user = user;
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
       Global_User? dbGlobalUser = GetGlobalUser(_user);

        if (dbGlobalUser != null && !string.IsNullOrWhiteSpace(Item.Password))
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
