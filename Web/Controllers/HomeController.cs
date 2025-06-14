using System.Diagnostics;
using Application.Shared.ViewModels;

namespace SecretSanta.Controllers;

public sealed class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public IActionResult Index(string? successMessage = null)
    {
        HomeModel.SuccessMessage = successMessage;
        return View(HomeModel);
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