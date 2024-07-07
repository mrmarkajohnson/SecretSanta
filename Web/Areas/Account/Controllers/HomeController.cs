using Application.Santa.Areas.Account.Commands;
using Application.Santa.Areas.Account.Queries;
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
            //This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
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
                return RedirectToPage("./Lockout");
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

    private void SetInvalidLogin()
    {
        ModelState.Clear();
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
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