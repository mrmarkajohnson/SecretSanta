using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.ViewLayer.Models;
using System.Diagnostics;
using Web.Controllers;

namespace SecretSanta.Controllers;

public class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public IActionResult Index(string? successMessage = null)
    {
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