using Application.Santa.Areas.Account.Queries;
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


    protected async Task<ISantaUser?> GetCurrentUser()
    {
        return await new GetCurrentUserQuery(User, UserManager, SignInManager).Handle();
    }
}
