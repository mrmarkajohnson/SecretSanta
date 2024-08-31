using Application.Santa.Areas.GiftingGroup.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.ViewLayer.Models;
using System.Diagnostics;
using ViewLayer.Models.Home;
using Web.Controllers;

namespace SecretSanta.Controllers;

public class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index(string? successMessage = null)
    {
        var model = new HomeVm { SuccessMessage = successMessage };

        try
        {
            model.CurrentUser = await GetCurrentUser(true);
            if (model.CurrentUser != null)
            {
                model.GiftingGroups = await Send(new GetUserGiftingGroupsQuery());
            }
        }
        catch { }

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}