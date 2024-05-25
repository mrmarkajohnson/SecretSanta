using Application.Santa.Areas.Account.Queries;
using Application.Santa.Global;
using Global.Abstractions.Global;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class BaseController : Controller
{
    public BaseController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        UserManager = userManager;
        SignInManager = signInManager;
    }

    protected UserManager<IdentityUser> UserManager { get; private init; }
    protected SignInManager<IdentityUser> SignInManager { get; private init; }

    protected async Task<ISantaUser?> GetCurrentUser(bool unHashIdentification)
    {
        return await Send(new GetCurrentUserQuery(User, UserManager, SignInManager, unHashIdentification));
    }

    protected async Task<TItem> Send<TItem>(BaseQuery<TItem> query)
    {
        return await query.Handle();
    }

    protected async Task<bool> Send<TItem>(BaseAction<TItem> action)
    {
        return await action.Handle();
    }

    protected async Task<ICommandResult<TItem>> Send<TItem>(BaseCommand<TItem> command)
    {
        ICommandResult<TItem> commandResult = await command.Handle();

        foreach (var error in commandResult.Validation.Errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return commandResult;
    }
}
