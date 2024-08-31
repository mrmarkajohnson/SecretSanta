using Application.Santa.Areas.Account.Queries;
using Application.Santa.Global;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Global.Abstractions.Global;
using Global.Abstractions.Global.Account;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class BaseController : Controller
{
    public BaseController(IServiceProvider services, SignInManager<IdentityUser> signInManager)
    {
        Services = services;
        SignInManager = signInManager;

        Mapper = services.GetRequiredService<IMapper>();

        if (Mapper == null)
        {
            throw new ArgumentException("Mapper cannot be null");
        }
    }

    public IServiceProvider Services { get; }
    protected SignInManager<IdentityUser> SignInManager { get; private init; }
    protected IMapper Mapper { get; set; }

    public IActionResult RedirectWithMessage(IForm model, string successMessage)
    {
        return RedirectWithMessage(model.ReturnUrl ?? Url.Content("~/"), successMessage);
    }

    public IActionResult RedirectWithMessage(string url, string successMessage)
    {
        string addQuery = url.Contains("?") ? "&" : "?";
        return LocalRedirect($"{url}{addQuery}SuccessMessage={successMessage}");
    }

    protected async Task<ISantaUser?> GetCurrentUser(bool unHashIdentification)
    {
        return await Send(new GetCurrentUserQuery(unHashIdentification));
    }

    protected async Task<TItem> Send<TItem>(BaseQuery<TItem> query)
    {
        TItem? result = await query.Handle(Services, User);
        return result;
    }

    protected async Task<bool> Send<TItem>(BaseAction<TItem> action)
    {
        return await action.Handle(Services, User);
    }

    protected async Task<ICommandResult<TItem>> Send<TItem>(BaseCommand<TItem> command, AbstractValidator<TItem>? validator)
    {
        ValidationResult validationResult = Validate(command.Item, validator);

        if (ModelState.IsValid && validationResult.IsValid)
        {
            command.Validator = validator;
            command.Validation = validationResult;

            ICommandResult<TItem> commandResult = await command.Handle(Services, User);
            AddErrorsToModelState(commandResult);

            return commandResult;
        }
        else
        {
            return new CommandResult<TItem>
            {
                Item = command.Item,
                Success = false,
                Validation = validationResult
            };
        }
    }

    private ValidationResult Validate<TItem>(TItem item, AbstractValidator<TItem>? validator)
    {
        ValidationResult validationResult = new();

        if (validator != null)
        {
            validationResult = validator.Validate(item);
            AddErrorsToModelState(validationResult);
        }

        return validationResult;
    }

    protected bool ValidateItem<TItem>(TItem item, AbstractValidator<TItem> validator)
    {
        ValidationResult validationResult = Validate(item, validator);
        return ModelState.IsValid && validationResult.IsValid;
    }

    private void AddErrorsToModelState<TItem>(ICommandResult<TItem> commandResult)
    {
        AddErrorsToModelState(commandResult.Validation);
    }

    private void AddErrorsToModelState(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }

    protected async Task<IActionResult> RedirectIfLockedOut(string viewName, ICheckLockout model)
    {
        if (model is ICheckLockout checkLockout && checkLockout.LockedOut)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return LocalRedirect(Url.Action("LockedOut", "Home", new { Area = "Account" }));
        }
        else
        {
            return View(viewName, model);
        }
    }

    protected async Task<IActionResult> RedirectToLogin(HttpRequest request)
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme); // just in case
        return LocalRedirect(Url.Action("Login", "Home", new { Area = "Account", ReturnUrl = request.Path.ToString(), TimedOut = true }));
    }
}
