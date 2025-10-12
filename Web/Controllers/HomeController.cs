using Application.Shared.ViewModels;
using Global.Settings;
using System.Diagnostics;

namespace Web.Controllers;

public sealed class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public IActionResult Index(string? successMessage = null)
    {
        HomeModel.SuccessMessage = successMessage;
        HomeModel.InvitationError = TempData[TempDataNames.InvitationError]?.ToString();
        TempData.Remove(TempDataNames.InvitationError); // just in case
        return View(HomeModel);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult FAQs()
    {
        return View();
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