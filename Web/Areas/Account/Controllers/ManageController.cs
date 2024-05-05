using Application.Santa.Areas.Account.Commands;
using Global.Abstractions.Global;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models.Account;

namespace Web.Areas.Account.Controllers;

[Area("Account")]
public class ManageController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly SignInManager<IdentityUser> _signInManager;

    public ManageController(UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _userStore = userStore;
        _signInManager = signInManager;
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
                CreateSantaUserCommand(model, _userManager, _userStore, _signInManager).Handle();

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

        return View();
    }

    [HttpGet]
    public IActionResult SetSecurityQuestions()
    {
        var model = new SetSecurityQuestionsVm();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SetSecurityQuestions(SetSecurityQuestionsVm model)
    {
        model.ReturnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            ICommandResult<ISecurityQuestions> commandResult = await new
                SetSecurityQuestionsCommand(model, User, _userManager, _signInManager).Handle();

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

        return View();
    }
}
