using Application.Areas.Account.Commands;
using Application.Areas.Account.Queries;
using Global.Abstractions.Areas.Account;
using Global.Extensions.System;
using Global.Settings;
using Microsoft.AspNetCore.Authorization;
using ViewLayer.Models.Account;

namespace Web.Areas.Account.Controllers;

[Area("Account")]
public sealed class ManageController : BaseController
{
    private readonly IUserStore<IdentityUser> _userStore;

    public ManageController(IServiceProvider services, SignInManager<IdentityUser> signInManager, IUserStore<IdentityUser> userStore)
        : base(services, signInManager)
    {
        _userStore = userStore;
    }

    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        var model = new RegisterVm
        {
            ReturnUrl = returnUrl,
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        //model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            var commandResult = await Send(new CreateSantaUserCommand<RegisterVm>(model, _userStore),
                new RegisterVmValidator());

            if (commandResult.Success)
            {
                return RedirectWithMessage(model.ReturnUrl ?? string.Empty, "Registered Successfully");
            }
        }

        return View(model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> SetSecurityQuestions()
    {
        if (SignInManager.IsSignedIn(User))
        {
            ISecurityQuestions? currentSecurityQuestions = await Send(new GetSecurityQuestionsQuery());            
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
                Greetings = greetings,
                Greeting = currentGreeting
            };

            if (currentSecurityQuestions != null)
            {
                Mapper.Map(currentSecurityQuestions, model);
            }

            return View(model);
        }
        else
        {
            return await RedirectToLogin(HttpContext.Request);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SetSecurityQuestions(SetSecurityQuestionsVm model)
    {
        if (ModelState.IsValid)
        {
            if (!model.Update) // in case the user removed it
            {
                ISecurityQuestions? currentSecurityQuestions = await Send(new GetSecurityQuestionsQuery());
                model.Update = currentSecurityQuestions?.SecurityQuestionsSet == true; // current password must be confirmed
            }

            var commandResult = await Send(new SetSecurityQuestionsCommand<SetSecurityQuestionsVm>(model),
                new SetSecurityQuestionsVmValidator());

            if (commandResult.Success)
            {
                return RedirectWithMessage(model, "Security Questions Set Successfully");
            }
        }

        return await RedirectIfLockedOut("SetSecurityQuestions", model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UpdateDetails(string? returnUrl = null)
    {
        if (SignInManager.IsSignedIn(User))
        {
            var currentUser = await GetCurrentUser(true);

            if (currentUser == null)
            {
                return await RedirectToLogin(HttpContext.Request);
            }

            var model = new UpdateDetailsVm
            {
                ReturnUrl = returnUrl
            };

            Mapper.Map(currentUser, model);
            return View(model);
        }
        else
        {
            return await RedirectToLogin(HttpContext.Request);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateDetails(UpdateDetailsVm model)
    {
        if (ModelState.IsValid)
        {
            var commandResult = await Send(new UpdateAccountDetailsCommand<UpdateDetailsVm>(model, _userStore),
                new UpdateDetailsVmValidator());

            if (commandResult.Success)
            {
                return RedirectWithMessage(model, "Details Updated Successfully");
            }
        }

        return await RedirectIfLockedOut("UpdateDetails", model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string? returnUrl = null)
    {
        if (SignInManager.IsSignedIn(User))
        {
            var currentUser = await GetCurrentUser(true);

            if (currentUser == null)
            {
                return await RedirectToLogin(HttpContext.Request);
            }

            var model = new ChangePasswordVm
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }
        else
        {
            return await RedirectToLogin(HttpContext.Request);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordVm model)
    {
        if (ModelState.IsValid)
        {
            var commandResult = await Send(new ChangePasswordCommand<ChangePasswordVm>(model),
                new ChangePasswordVmValidator());

            if (commandResult.Success)
            {
                return RedirectWithMessage(model, "Details Updated Successfully");
            }
        }

        return await RedirectIfLockedOut("ChangePassword", model);
    }
}
