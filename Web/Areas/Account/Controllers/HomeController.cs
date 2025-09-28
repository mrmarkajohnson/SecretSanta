using Application.Areas.Account.Commands;
using Application.Areas.Account.Queries;
using Application.Areas.Account.ViewModels;
using Application.Areas.GiftingGroup.Queries;
using Global.Validation;
using Microsoft.AspNetCore.Authentication;
using static Global.Settings.GlobalSettings;

namespace Web.Areas.Account.Controllers;

[Area(AreaNames.Account)]
public sealed class HomeController : AccountBaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) 
        : base(services, signInManager)
    {
    }    

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null, bool timedOut = false)
    {
        if (SignInManager.IsSignedIn(User))
        {
            return RedirectToLocalUrl(nameof(Index), nameof(HomeController), "");
        }

        var model = new LoginVm
        {
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            ReturnUrl = returnUrl,
            TimedOut = timedOut,
            RememberMe = true
        };

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm model)
    {
        //model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            var result = await Send(new LoginQuery(model));

            if (result.Succeeded)
            {
                string? invitationMessage = await HandleInvitation();
                string message = "Logged in successfully." + (invitationMessage.IsNotEmpty() ? $" {invitationMessage}" : "");
                return RedirectWithMessage(model, message);
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVm model)
    {
        ModelState.Clear();

        var commandResult = await Send(new ForgotPasswordCommand<ForgotPasswordVm>(model), new ForgotPasswordVmValidator());

        if (commandResult.Success && model.PasswordResetSuccessfully)
        {
            return RedirectWithMessage(model, "Password reset successfully.");
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

    [HttpGet]
    public async Task<IActionResult> AcceptInvitation(string id)
    {
        TempData[InvitationId] = id;

        if (SignInManager.IsSignedIn(User))
        {
            string? message = await HandleInvitation(true);
            return RedirectToLocalUrl(nameof(Index), nameof(HomeController), "", new { successMessage = message });
        }
        else
        {
            var invitation = await Send(new GetInvitationQuery(id));

            if (invitation == null)
            {
                TempData.Remove(InvitationId);
            }

            if (invitation?.ToSantaUserKey == null)
            {
                return RedirectToLocalUrl(nameof(Index), nameof(HomeController), "");
            }
            else
            {
                return RedirectToLocalUrl(nameof(Login), nameof(HomeController), "");
            }
        }
    }
}