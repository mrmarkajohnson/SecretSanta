using Application.Areas.Account.BaseModels;
using Application.Areas.Account.Commands;
using Application.Areas.Account.Queries;
using Application.Areas.Account.ViewModels;
using Global.Abstractions.Areas.Account;
using Global.Names;
using Global.Settings;
using Microsoft.AspNetCore.Authorization;
using static Global.Settings.GlobalSettings;

namespace Web.Areas.Account.Controllers;

[Area(AreaNames.Account)]
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
        returnUrl ??= Url.Action(nameof(SetSecurityQuestions));

        string? invitationWaitMessage = TempData.Peek(TempDataNames.InvitationWaitMessage)?.ToString();

        if (invitationWaitMessage.IsNotEmpty())
        {
            invitationWaitMessage += " You can review it after registering and setting your security questions.";
        }

        var model = new RegisterVm
        {
            ReturnUrl = returnUrl,
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            InvitationWaitMessage = invitationWaitMessage
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        //model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            var commandResult = await Send(new CreateSantaUserCommand<RegisterVm>(model, _userStore),
                new RegisterVmValidator());

            if (commandResult.Success)
            {
                return RedirectWithMessage(model.ReturnUrl ?? string.Empty, "Registered successfully.");
            }
        }

        return View(model);
    }    

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> SetSecurityQuestions()
    {
        if (SignedIn())
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

            string? invitationWaitMessage = TempData.Peek(TempDataNames.InvitationWaitMessage)?.ToString();

            if (invitationWaitMessage.IsNotEmpty())
            {
                invitationWaitMessage += " You can review it after setting your security questions.";
            }

            var model = new SetSecurityQuestionsVm
            {
                Greetings = greetings,
                Greeting = currentGreeting,
                InvitationWaitMessage = invitationWaitMessage
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
    [Authorize]
    [ValidateAntiForgeryToken]
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
                HandleInvitation(model);
                return RedirectWithMessage(model, "Security questions set successfully.");
            }
        }

        return await RedirectIfLockedOut("SetSecurityQuestions", model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UpdateDetails(string? returnUrl = null)
    {
        if (SignedIn())
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDetails(UpdateDetailsVm model)
    {
        if (ModelState.IsValid)
        {
            var commandResult = await Send(new UpdateAccountDetailsCommand<UpdateDetailsVm>(model, _userStore),
                new UpdateDetailsVmValidator());

            if (commandResult.Success)
            {
                return RedirectWithMessage(model, "Details updated successfully.");
            }
        }

        return await RedirectIfLockedOut("UpdateDetails", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ShowNameVariations(SantaUser model)
    {
        return PartialView("_ShowNameVariations", model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string? returnUrl = null)
    {
        if (SignedIn())
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordVm model)
    {
        if (ModelState.IsValid)
        {
            var commandResult = await Send(new ChangePasswordCommand<ChangePasswordVm>(model),
                new ChangePasswordVmValidator());

            if (commandResult.Success)
            {
                return RedirectWithMessage(model, "Password changed successfully.");
            }
        }

        return await RedirectIfLockedOut("ChangePassword", model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendEmailConfirmation()
    {
        await Send(new SendEmailConfirmationCommand(), null);
        return Ok("Confirmation request sent successfully.");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ConfirmEmail(string id)
    {
        try
        {
            await Send(new ConfirmEmailCommand(id), null);
            return RedirectWithMessage(Url.Action(nameof(EmailPreferences)), $"{UserDisplayNames.Email} confirmed successfully.");
        }
        catch (ArgumentException ex)
        {
            return AccessDenied(ex.Message);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> EmailPreferences()
    {
        var emailDetails = await Send(new GetEmailDetailsQuery());
        var model = Mapper.Map<EmailPreferencesVm>(emailDetails);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EmailPreferences(EmailPreferencesVm model)
    {
        ModelState.Clear();
        
        var commandResult = await Send(new SetEmailPreferencesCommand<UserEmailDetails>(model, _userStore), new EmailDetailsValidator());
        if (commandResult.Success)
        {
            return RedirectWithMessage(model, "Preferences saved successfully.");
        }

        return View(model);
    }
}
