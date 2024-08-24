using Global.Abstractions.Global.Account;
using Global.Extensions.Exceptions;
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
        if (!Validation.IsValid)
        {
            return await Result();
        }
        else if (string.IsNullOrWhiteSpace(Item.Password)) // just in case
        {
            AddValidationError(nameof(Item.Password), "Please enter a new password.");
            return await Result();
        }

        EnsureSignedIn(_user, SignInManager);

        string? userId = UserManager.GetUserId(_user);
        if (userId != null)
        {
            Global_User? dbGlobalUser = GetGlobalUser(userId);

            if (dbGlobalUser != null)
            {
                bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, dbGlobalUser);
                if (!passwordCorrect || !Validation.IsValid)
                {
                    return await Result();
                }
                else
                {
                    await ChangePassword(dbGlobalUser);
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

        return await Result();
    }
}
