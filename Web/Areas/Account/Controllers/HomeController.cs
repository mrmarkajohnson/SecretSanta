using Application.Santa.Areas.Account.Commands;
using Application.Santa.Areas.Account.Queries;
using Global.Abstractions.Global;
using Global.Abstractions.Santa.Areas.Account;
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
            var result = await SignInManager.PasswordSignInAsync(model.EmailOrUserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return LocalRedirect(model.ReturnUrl);
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
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        model.Password = "";
        return View(model);
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
        
        if (string.IsNullOrWhiteSpace(model.EmailOrUserName) || string.IsNullOrWhiteSpace(model.Forename))
        {
            SetDetailsNotRecognisedError(model);
        }
        else
        {
            ISantaUser? user = await new GetUserQuery(model.EmailOrUserName, model.Forename).Handle();
            if (user == null || user.UserName == null)
            {
                SetDetailsNotRecognisedError(model);
            }
            else 
            {
                ISecurityQuestions? securityQuestions = await new GetSecurityQuestionsQuery(user.UserName, UserManager, SignInManager).Handle();

                if (securityQuestions == null)
                {
                    SetDetailsNotRecognisedError(model);
                }
                else if (!model.SecurityQuestionsSet)
                {
                    model.SecurityQuestion1 = securityQuestions.SecurityQuestion1;
                    model.SecurityHint1 = securityQuestions.SecurityHint1;
                    model.SecurityQuestion2 = securityQuestions.SecurityQuestion2;
                    model.SecurityHint2 = securityQuestions.SecurityHint2;
                    model.ShowSecurityQuestions = false;
                    model.ShowSecurityQuestions = true;
                    model.ResetPassword = false;
                }
                else if (model.SecurityAnswer1?.ToLower() != securityQuestions.SecurityAnswer1?.ToLower()
                    || model.SecurityAnswer2?.ToLower() != securityQuestions.SecurityAnswer2?.ToLower())
                {
                    ModelState.AddModelError(string.Empty, "Security answers did not match.");
                    model.ShowBasicDetails = false;
                    model.ShowSecurityQuestions = true;
                    model.ResetPassword = false;
                }
                else if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ConfirmPassword))
                {                    
                    model.ShowBasicDetails = false;
                    model.ShowSecurityQuestions = false;
                    model.ResetPassword = true;
                }
                else if (model.ConfirmPassword != model.Password)
                {
                    ModelState.AddModelError(string.Empty, "Passwords did not match.");
                    model.ShowBasicDetails = false;
                    model.ShowSecurityQuestions = false;
                    model.ResetPassword = true;
                }
                else
                {
                    ICommandResult<ISantaUser> commandResult = await new
                        ChangePasswordCommand(model, user, UserManager, SignInManager).Handle();

                    if (commandResult.Success)
                    {
                        model.ReturnUrl ??= Url.Content("~/");
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        foreach (var error in commandResult.Validation.Errors)
                        {
                            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        }
                    }
                }
            }
        }

        return View(model);
    }

    private void SetDetailsNotRecognisedError(ForgotPasswordVm model)
    {
        ModelState.AddModelError(string.Empty, "Details not recognised.");
        model.ShowBasicDetails = true;
        model.ShowSecurityQuestions = false;
        model.ResetPassword = false;
    }
}