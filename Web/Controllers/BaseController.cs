using Application.Santa.Areas.Account.Queries;
using Application.Santa.Global;
using FluentValidation;
using FluentValidation.Results;
using Global.Abstractions.Global;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class BaseController : Controller
{
    public BaseController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        UserManager = userManager;
        SignInManager = signInManager;
    }

    protected UserManager<IdentityUser> UserManager { get; private init; }
    protected SignInManager<IdentityUser> SignInManager { get; private init; }

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
        return await Send(new GetCurrentUserQuery(User, UserManager, SignInManager, unHashIdentification));
    }

    protected async Task<TItem> Send<TItem>(BaseQuery<TItem> query)
    {
        return await query.Handle();
    }

    protected async Task<bool> Send<TItem>(BaseAction<TItem> action)
    {
        return await action.Handle();
    }

    protected async Task<ICommandResult<TItem>> Send<TItem>(BaseCommand<TItem> command, AbstractValidator<TItem>? validator)
    {
        ValidationResult validationResult = Validate(command.Item, validator);

        if (ModelState.IsValid && validationResult.IsValid)
        {
            command.Validator = validator;
            command.Validation = validationResult;

            ICommandResult<TItem> commandResult = await command.Handle();
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
}
