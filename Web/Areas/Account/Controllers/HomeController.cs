using Application.Santa.Areas.Account.Commands;
using Application.Santa.Areas.Account.Queries;
using Global.Abstractions.Extensions;
using Global.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models.Account;
using Web.Controllers;

namespace Web.Areas.Account.Controllers;

[Area("Account")]
public class HomeController : BaseController
{
    public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : base(userManager, signInManager)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        var model = new LoginVm
        {
            EmailOrUserName = "",
            Password = "",
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            ReturnUrl = returnUrl ?? Url.Content("~/")
        };

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);        

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm model)
    {
        model.ReturnUrl ??= Url.Content("~/");

        //model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            var result = await Send(new LoginQuery(model, SignInManager));
            
            if (result.Succeeded)
            {
                return RedirectWithMessage(model, "Logged In Successfully");
            }
            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToPage("./LoginWith2fa", new { model.ReturnUrl,model.RememberMe });
            //}
            if (result.IsLockedOut)
            {
                model.LockedOut = true;
                SetInvalidLogin(true);
            }
            else
            {
                SetInvalidLogin();
            }
        }
        else
        {
            SetInvalidLogin();
        }

        model.Password = "";
        return View(model);
    }

    private void SetInvalidLogin(bool lockedOut = false)
    {
        ModelState.Clear();
        string message = "Invalid login attempt." + (lockedOut ? $" This account is now locked out for {IdentityVal.Lockouts.DefaultLockoutTimeSpan.GetDescription()}." : "");
        ModelState.AddModelError(string.Empty, message);
    }

    [HttpGet]
    public IActionResult ForgotPassword(string? returnUrl = null)
    {
        var model = new ForgotPasswordVm
        {
            EmailOrUserName = "",
            Forename = "",
            Password = "",
            ConfirmPassword = "",
            ShowBasicDetails = true,
            ShowSecurityQuestions = false,
            ResetPassword = false,
            ReturnUrl = returnUrl ?? Url.Content("~/")
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVm model)
    {
        ModelState.Clear();

        var commandResult = await Send(new ForgotPasswordCommand<ForgotPasswordVm>(model, UserManager, SignInManager), new ForgotPasswordVmValidator());

        if (commandResult.Success && model.PasswordResetSuccessfully)
        {
            model.ReturnUrl ??= Url.Content("~/");
            return RedirectWithMessage(model, "Password Reset Successfully");
        }

        return View(model);
    }
}