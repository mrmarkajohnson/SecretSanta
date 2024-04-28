using Application.Santa.Areas.Account.Queries;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.ViewLayer.Models;
using System;
using System.Diagnostics;
using ViewLayer.Models.Home;

namespace SecretSanta.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly SignInManager<IdentityUser> _signInManager;

    public HomeController(ILogger<HomeController> logger, 
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _userStore = userStore;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeVm();
        model.CurrentUser = await new GetCurrentUserQuery(User, _userManager, _signInManager).Handle();
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