using Application.Areas.Account.Queries;
using Application.Areas.GiftingGroup.Queries;
using Application.Areas.Home.ViewModels;
using Application.Areas.Messages.Commands;
using Application.Shared.Requests;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.ViewModels;
using Global.Extensions.Exceptions;
using Global.Helpers;
using Global.Settings;
using Microsoft.AspNetCore.Authentication;
using Web.Helpers;
using static Global.Settings.GlobalSettings;
using AccountControllers = Web.Areas.Account.Controllers;
using GroupControllers = Web.Areas.GiftingGroup.Controllers;

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

        HomeModel = new HomeVm();
        ViewData["LayoutViewModel"] = HomeModel;
    }

    public IServiceProvider Services { get; }
    protected SignInManager<IdentityUser> SignInManager { get; private init; }
    protected IMapper Mapper { get; set; }
    public HomeVm HomeModel { get; set; }

    public async Task SetHomeModel()
    {
        HomeModel ??= new HomeVm();

        try
        {
            HomeModel.CurrentUser = await GetCurrentUser(true);
            HomeModel.GiftingGroups = await Send(new GetUserGiftingGroupsQuery());
        }
        catch (NotSignedInException) { }
        catch (AccessDeniedException) { }
    }

    protected async Task<ISantaUser> GetCurrentUser(bool unHashIdentification)
    {
        return await Send(new GetCurrentUserQuery(unHashIdentification));
    }

    protected void EnsureSignedIn()
    {
        if (!SignedIn())
        {
            throw new NotSignedInException();
        }
    }

    public bool SignedIn()
    {
        return SignInManager.IsSignedIn(User);
    }

    #region Send requests

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

    #endregion Send requests

    #region Validation and ModelState

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

    protected IActionResult SuccessOrFailureMessage<T>(ICommandResult<T> commandResult, string fallbackSuccessMessage)
    {
        if (commandResult.Success)
        {
            return Ok(commandResult.SuccessMessage ?? fallbackSuccessMessage);
        }
        else
        {
            return FirstValidationError(commandResult);
        }
    }

    protected ObjectResult FirstValidationError<T>(ICommandResult<T> result)
    {
        string message = result?.Validation?.Errors?.Count > 0 ? result.Validation.Errors[0].ErrorMessage : "An error occurred.";
        return ErrorMessageResult(message);
    }

    protected ObjectResult ErrorMessageResult(string message)
    {
        return StatusCode(StatusCodes.Status422UnprocessableEntity, message);
    }

    #endregion Validation and ModelState

    #region Redirection and URLs

    public IActionResult RedirectWithMessage(IFormVm model, string successMessage)
    {
        return RedirectWithMessage(model.ReturnUrl ?? Url.Content("~/"), successMessage);
    }

    public IActionResult RedirectWithMessage(string? url, string successMessage)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            url = Url.Content("~/");
        }

        if (url.EndsWith("Controller") && !url.EndsWith("/Controller"))
        {
            url = url.TrimEnd("Controller") + "/Index";
        }

        string addQuery = UrlHelper.ParameterDelimiter(url);
        return RedirectToLocalUrl($"{url}{addQuery}successMessage={successMessage}");
    }

    protected async Task<IActionResult> RedirectIfLockedOut(string viewName, ICheckLockout model)
    {
        if (model is ICheckLockout checkLockout && checkLockout.LockedOut)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return RedirectToLocalUrl<AccountControllers.HomeController>(nameof(AccountControllers.HomeController.LockedOut), AreaNames.Account);
        }
        else
        {
            return View(viewName, model);
        }
    }

    protected async Task<IActionResult> RedirectToLogin(HttpRequest request)
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme); // just in case
        return RedirectToLocalUrl<AccountControllers.HomeController>(nameof(AccountControllers.HomeController.Login), AreaNames.Account, new { ReturnUrl = request.Path.ToString(), TimedOut = true });
    }

    /// <summary>
    /// Avoid annoying null reference errors
    /// </summary>
    protected LocalRedirectResult RedirectToLocalUrl<TController>(string action, string area, object? values = null) where TController : BaseController
    {
        string localUrl = GetLocalUrl<TController>(action, area, values);
        return RedirectToLocalUrl(localUrl);
    }

    /// <summary>
    /// Avoid annoying null reference errors
    /// </summary>
    protected LocalRedirectResult RedirectToLocalUrl(string? localUrl)
    {
        return LocalRedirect(localUrl ?? "");
    }

    [HttpGet]
    public IActionResult AccessDenied(string? message = null)
    {
        return View("AccessDenied", message);
    }

    [HttpGet]
    public IActionResult NotFound(string? message = null)
    {
        return View("NotFound", message);
    }

    protected IActionResult RedirectHome()
    {
        return RedirectToLocalUrl<HomeController>(nameof(HomeController.Index), AreaNames.None);
    }

    protected string GetFullUrl<TController>(string action, string area, object? values = null) where TController : BaseController
    {
        return Url.Action(Request, action, typeof(TController).Name, area, values);
    }

    public string GetLocalUrl<TController>(string action, string area, object? values = null) where TController : BaseController
    {
        return Url.Action(action, typeof(TController).Name, area, values);
    }

    protected string GetParticipateUrl()
    {
        return GetFullUrl<GroupControllers.ParticipateController>(nameof(GroupControllers.ParticipateController.Index), AreaNames.GiftingGroup);
    }

    protected string GetReviewInvitationUrl(string invitationId)
    {
        return GetFullUrl<GroupControllers.ManageController>(nameof(GroupControllers.ManageController.ReviewInvitation), AreaNames.GiftingGroup, new { invitationId });
    }

    #endregion Redirection and URLs

    protected bool AjaxRequest()
    {
        return HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }

    public async Task<IActionResult> MarkMessageRead(int messageKey, int? messageRecipientKey)
    {
        try
        {
            await Send(new MarkMessageReadCommand(messageKey, messageRecipientKey), null);
        }
        catch
        {
            // no point throwing an exception here
        }

        return Ok();
    }

    protected void HandleInvitation(IFormVm model)
    {
        string? invitationId = TempData.Peek(TempDataNames.InvitationId)?.ToString();

        if (invitationId != null)
        {
            model.ReturnUrl = GetReviewInvitationUrl(invitationId);
        }
    }
}
