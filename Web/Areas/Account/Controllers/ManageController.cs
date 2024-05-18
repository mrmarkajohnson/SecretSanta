using Application.Santa.Areas.Account.Commands;
using Application.Santa.Areas.Account.Queries;
using Global.Abstractions.Global;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models.Account;
using Web.Controllers;

namespace Web.Areas.Account.Controllers;

[Area("Account")]
public class ManageController : BaseController
{
    private readonly IUserStore<IdentityUser> _userStore;

    public ManageController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserStore<IdentityUser> userStore) 
        : base(userManager, signInManager)
    {
        _userStore = userStore;
    }

    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        var model = new RegisterVm
        {
            ReturnUrl = returnUrl ?? Url.Content("~/"),
            Forename = "",
            Surname = "",
            Password = "",
            ConfirmPassword = "",
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        model.ReturnUrl ??= Url.Content("~/");
        //model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            ICommandResult<IRegisterSantaUser> commandResult = await new
                CreateSantaUserCommand(model, UserManager, _userStore, SignInManager).Handle();

            if (commandResult.Success)
            {
                if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                {
                    return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = model.ReturnUrl });
                }
                else
                {
                    return Redirect(model.ReturnUrl);
                }
            }
            else
            {
                foreach (var error in commandResult.Validation.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> SetSecurityQuestions(bool update = false)
    {
        if (SignInManager.IsSignedIn(User))
        { 
            var model = new SetSecurityQuestionsVm();

            ISecurityQuestions? currentSecurityQuestions = await new GetSecurityQuestionsQuery(User, UserManager, SignInManager).Handle();

            if (currentSecurityQuestions?.SecurityQuestionsSet == true)
            {
                model = new SetSecurityQuestionsVm
                {
                    SecurityQuestion1 = currentSecurityQuestions.SecurityQuestion1,
                    SecurityAnswer1 = currentSecurityQuestions.SecurityAnswer1,
                    SecurityHint1 = currentSecurityQuestions.SecurityHint1,
                    SecurityQuestion2 = currentSecurityQuestions.SecurityQuestion2,
                    SecurityAnswer2 = currentSecurityQuestions.SecurityAnswer2,
                    SecurityHint2 = currentSecurityQuestions.SecurityHint2,
                    Update = update
                };
            }

            return View(model);
        }
        else
        {
            return Redirect(Url.Action("Error"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> SetSecurityQuestions(SetSecurityQuestionsVm model)
    {
        model.ReturnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            ICommandResult<ISecurityQuestions> commandResult = await new
                SetSecurityQuestionsCommand(model, User, UserManager, SignInManager).Handle();

            if (commandResult.Success)
            {
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

        return View(model);
    }
}
