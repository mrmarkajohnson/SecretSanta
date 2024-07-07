using Application.Santa.Areas.Account.Commands;
using Application.Santa.Areas.Account.Queries;
using Global.Abstractions.Extensions;
using Global.Abstractions.Global;
using Global.Settings;
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
            Greeting = ""
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
            var commandResult = await Send(new CreateSantaUserCommand<RegisterVm>(model, UserManager, _userStore, SignInManager),
                new RegisterVmValidator());

            if (commandResult.Success)
            {
                if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                {
                    return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = model.ReturnUrl });
                }
                else
                {
                    return RedirectWithMessage(model.ReturnUrl, "Registered Successfully");
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
            ISecurityQuestions? currentSecurityQuestions = await Send(new GetSecurityQuestionsQuery(User, UserManager, SignInManager));            
            string? currentGreeting = currentSecurityQuestions?.Greeting;

            if (string.IsNullOrWhiteSpace(currentGreeting))
            {
                var user = await GetCurrentUser(true);
                currentGreeting = user?.Greeting;
            }

            List<string> greetings = new();

            if (string.IsNullOrWhiteSpace(currentGreeting)) // just in case
            {
                greetings = Greetings.Messages.GetNFromList(3);
                currentGreeting = greetings[0];
            }
            else
            {
                greetings.Add(currentGreeting);
                greetings.AddRange(Greetings.Messages.Where(x => x != currentGreeting).ToList().GetNFromList(2)); // add 2 others to choose from
            }

            var model = new SetSecurityQuestionsVm
            {
                SecurityQuestion1 = currentSecurityQuestions?.SecurityQuestion1,
                SecurityAnswer1 = null,
                SecurityHint1 = currentSecurityQuestions?.SecurityHint1,
                SecurityQuestion2 = currentSecurityQuestions?.SecurityQuestion2,
                SecurityAnswer2 = null,
                SecurityHint2 = currentSecurityQuestions?.SecurityHint2,
                Update = update && currentSecurityQuestions?.SecurityQuestionsSet == true,
                Greetings = greetings,
                Greeting = currentGreeting
            };

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
            var commandResult = await Send(new SetSecurityQuestionsCommand<SetSecurityQuestionsVm>(model, User, UserManager, SignInManager),
                new SetSecurityQuestionsVmValidator());

            if (commandResult.Success)
            {
                return RedirectWithMessage(model, "Security Questions Set Successfully");
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateDetails(string? returnUrl = null)
    {
        if (SignInManager.IsSignedIn(User))
        {
            var currentUser = await GetCurrentUser(true);

            if (currentUser == null)
            {
                return Redirect(Url.Action("Error"));
            }

            var model = new UpdateDetailsVm
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                Password = "",
                Email = currentUser.Email,
                Forename = currentUser.Forename,
                MiddleNames = currentUser.MiddleNames,
                Surname = currentUser.Surname,
                Greeting = "", // not needed
                ReturnUrl = returnUrl ?? Url.Content("~/")
            };

            return View(model);
        }
        else
        {
            return Redirect(Url.Action("Error"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateDetails(UpdateDetailsVm model)
    {
        model.ReturnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var commandResult = await Send(new UpdateAccountDetailsCommand<UpdateDetailsVm>(model, User, UserManager, _userStore, SignInManager),
                new UpdateDetailsVmValidator());

            if (commandResult.Success)
            {
                return RedirectWithMessage(model, "Details Updated Successfully");
            }
        }

        return View(model);
    }
}
