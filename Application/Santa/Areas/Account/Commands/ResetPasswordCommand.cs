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
        var globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.Id == _user.Id);

        if (globalUserDb != null && !string.IsNullOrWhiteSpace(Item.Password))
        {
            await ChangePassword(globalUserDb);
        }
        else
        {
            AddUserNotFoundError();
        }

        return await Result();
    }
}
