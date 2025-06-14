using Application.Areas.Account.Commands;
using Application.Areas.Account.Queries;
using Global.Validation;
using Microsoft.AspNetCore.Authentication;
using ViewModels.Models.Account;

namespace Web.Areas.Account.Controllers;

[Area("Account")]
public sealed class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null, bool timedOut = false)
    {
        var model = new LoginVm
        {
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            ReturnUrl = returnUrl,
            TimedOut = timedOut,
        };

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm model)
    {
        //model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            var result = await Send(new LoginQuery(model));

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

        model.Password = string.Empty;
        return await RedirectIfLockedOut("Login", model);
    }

    private void SetInvalidLogin(bool lockedOut = false)
    {
        ModelState.Clear();
        string message = "Invalid login attempt." + (lockedOut ? $" This account is now locked out for {IdentityVal.Lockouts.DefaultLockoutTimeSpan.GetDescription()}." : string.Empty);
        ModelState.AddModelError(string.Empty, message);
    }

    [HttpGet]
    public IActionResult ForgotPassword(string? returnUrl = null)
    {
        var model = new ForgotPasswordVm
        {
            ShowBasicDetails = true,
            ShowSecurityQuestions = false,
            ResetPassword = false,
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVm model)
    {
        ModelState.Clear();

        var commandResult = await Send(new ForgotPasswordCommand<ForgotPasswordVm>(model), new ForgotPasswordVmValidator());

        if (commandResult.Success && model.PasswordResetSuccessfully)
        {
            return RedirectWithMessage(model, "Password Reset Successfully");
        }

        return await RedirectIfLockedOut("ForgotPassword", model);
    }

    [HttpGet]
    public async Task<IActionResult> LockedOut()
    {
        if (SignInManager.IsSignedIn(User))
        {
            await SignInManager.SignOutAsync(); // just in case
        }

        return View();
    }
}