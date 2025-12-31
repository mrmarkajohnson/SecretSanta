using Application.Areas.GiftingGroup.Queries;
using Application.Areas.Participate.Commands;
using Application.Areas.Participate.Queries;
using Application.Areas.Participate.ViewModels;
using Application.Areas.Suggestions.Queries;
using Application.Areas.Suggestions.ViewModels;
using Global.Abstractions.Areas.Participate;
using Microsoft.AspNetCore.Authorization;
using static Global.Settings.GlobalSettings;

namespace Web.Areas.GiftingGroup.Controllers;

[Area(AreaNames.GiftingGroup)]
[Authorize]
public sealed class ParticipateController : BaseController
{
    public ParticipateController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index(int? calendarYear = null)
    {
        calendarYear ??= CurrentYear;

        if (AjaxRequest())
            return await GiftingGroupsGrid(calendarYear);

        UserGiftingGroupYearsVm model = await GetUserGiftingGroupYearsModel(calendarYear);
        return View(model);
    }

    private async Task<UserGiftingGroupYearsVm> GetUserGiftingGroupYearsModel(int? calendarYear)
    {
        calendarYear ??= CurrentYear;
        var groups = await Send(new UserGiftingGroupYearsQuery(calendarYear));
        var model = new UserGiftingGroupYearsVm(calendarYear.Value, groups);
        return model;
    }

    public async Task<IActionResult> GiftingGroupsGrid(int? calendarYear = null)
    {
        UserGiftingGroupYearsVm model = await GetUserGiftingGroupYearsModel(calendarYear);
        return PartialView("_GiftingGroupsGrid", model);
    }

    [HttpGet]
    public async Task<IActionResult> Year(int giftingGroupKey, int? calendarYear = null)
    {
        return await EditYearParticipation(giftingGroupKey, calendarYear);
    }

    private async Task<IActionResult> EditYearParticipation(int giftingGroupKey, int? calendarYear = null)
    {
        IManageUserGiftingGroupYear year = await Send(new ManageUserGiftingGroupYearQuery(giftingGroupKey, calendarYear));
        var model = Mapper.Map<ManageUserGiftingGroupYearVm>(year);
        model.IncludePreviousYears = model.PreviousYearsRequired > 0 && model.OtherMembersSelect.Count > 0;

        if (model.Recipient != null)
        {
            model.RecipientSuggestions = await GetRecipientSuggestions(giftingGroupKey, model.Recipient.HashedUserId, CurrentYear);
        }

        return View("Year", model);
    }

    [HttpGet]
    public async Task<IActionResult> RecipientSuggestionsGrid(int giftingGroupKey, string hashedUserId)
    {
        RecipientSuggestionsVm model = await GetRecipientSuggestions(giftingGroupKey, hashedUserId, CurrentYear);
        return PartialView("_RecipientSuggestionsGrid", model);
    }

    private async Task<RecipientSuggestionsVm> GetRecipientSuggestions(int giftingGroupKey, string hashedUserId, int calendarYear)
    {
        var suggestions = await Send(new GetRecipientSuggestionsQuery(giftingGroupKey, hashedUserId, calendarYear));
        return new RecipientSuggestionsVm(giftingGroupKey, hashedUserId, suggestions);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Year(ManageUserGiftingGroupYearVm model)
    {
        model.CurrencyCode ??= model.CurrencySymbol ??= ""; // avoid invalid model state
        ModelState.Clear();

        bool fromRadioButtons = model.SubmitIncludedChangeImmediately;
        var commandResult = await Send(new ParticipateInYearCommand<ManageUserGiftingGroupYearVm>(model), new UserGiftingGroupYearVmValidator());

        if (commandResult.Success)
        {
            string changed = model.Included ? "Included" : "Excluded";
            string message = $"{changed} successfully.";

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return RedirectWithMessage(model.ReturnUrl, message);
            }
            else if (fromRadioButtons)
            {
                return Ok(message);
            }
            else
            {
                return await EditYearParticipation(model.GiftingGroupKey);
            }
        }
        else
        {
            return FirstValidationError(commandResult);
        }
    }
}
