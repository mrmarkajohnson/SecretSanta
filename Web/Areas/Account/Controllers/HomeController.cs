using Application.Areas.Account.Commands;
using Application.Areas.Account.Queries;
using Application.Areas.Account.ViewModels;
using Application.Areas.GiftingGroup.Queries;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Global.Settings;
using Global.Validation;
using Microsoft.AspNetCore.Authentication;
using static Global.Settings.GlobalSettings;

namespace Web.Areas.Account.Controllers;

[Area(AreaNames.Account)]
public sealed class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) 
        : base(services, signInManager)
    {
    }    

    [HttpGet]
    public async Task<IActionResult> Login(string? returnUrl = null, bool timedOut = false)
    {
        if (SignInManager.IsSignedIn(User))
        {
            return RedirectHome();
        }

        string? invitationWaitMessage = TempData.Peek(TempDataNames.InvitationWaitMessage)?.ToString();

        if (invitationWaitMessage.IsNotEmpty())
        {
            invitationWaitMessage += " You can review it after logging in.";
        }

        var model = new LoginVm
        {
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            ReturnUrl = returnUrl,
            TimedOut = timedOut,
            RememberMe = true,
            InvitationWaitMessage = invitationWaitMessage
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
                HandleInvitation(model);
                return RedirectWithMessage(model, "Logged in successfully.");
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
    public async Task<IActionResult> ReviewInvitation(string id)
    {
        try
        {
            IReviewGroupInvitation invitation = await Send(new GetInvitationQuery(id));
            TempData[TempDataNames.InvitationId] = id;

            if (SignInManager.IsSignedIn(User))
            {
                if (HomeModel.CurrentUser?.SecurityQuestionsSet == false)
                {
                    SetInvitationWaitMessage(invitation);
                    return RedirectHome(); // this will force the user to set their security questions
                }
                else
                {
                    return LocalRedirect(GetReviewInvitationUrl(id));
                }
            }
            else
            {
                if (invitation.ToSantaUserKey == null)
                {
                    SetInvitationWaitMessage(invitation);
                    return RedirectToLocalUrl(nameof(Index), nameof(HomeController), "");
                }
                else
                {
                    SetInvitationWaitMessage(invitation);
                    return RedirectHome();
                }
            }
        }
        catch (NotFoundException nfx)
        {
            HandleInvitationError(nfx);
            return RedirectHome();
        }
        catch (AccessDeniedException adx)
        {
            HandleInvitationError(adx);
            return RedirectHome();
        }        
    }

    private void SetInvitationWaitMessage(IReviewGroupInvitation invitation)
    {
        TempData.Remove(TempDataNames.InvitationError);
        TempData[TempDataNames.InvitationWaitMessage] = $"You have a group invitation from {invitation.FromUser.DisplayName(false)}.";
    }

    private void HandleInvitationError(Exception ex)
    {
        TempData.Remove(TempDataNames.InvitationId);
        TempData.Remove(TempDataNames.InvitationWaitMessage);
        TempData[TempDataNames.InvitationError] = ex.Message;
    }
}