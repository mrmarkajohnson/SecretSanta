using Application.Areas.GiftingGroup.Queries;
using Application.Areas.Participate.Commands;
using Application.Areas.Participate.Queries;
using Application.Areas.Participate.ViewModels;
using Application.Areas.Suggestions.Queries;
using Application.Areas.Suggestions.ViewModels;
using Global.Abstractions.Areas.Participate;
using Microsoft.AspNetCore.Authorization;

namespace Web.Areas.GiftingGroup.Controllers;

[Area("GiftingGroup")]
[Authorize]
public sealed class ParticipateController : BaseController
{
    public ParticipateController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index()
    {
        if (AjaxRequest())
            return await GiftingGroupsGrid();

        var groups = await Send(new UserGiftingGroupYearsQuery());
        return View(groups);
    }

    public async Task<IActionResult> GiftingGroupsGrid()
    {
        var groups = await Send(new UserGiftingGroupYearsQuery());
        return PartialView("_GiftingGroupsGrid", groups);
    }

    [HttpGet]
    public async Task<IActionResult> Year(int giftingGroupKey)
    {
        return await EditYearParticipation(giftingGroupKey);
    }

    private async Task<IActionResult> EditYearParticipation(int giftingGroupKey)
    {
        IManageUserGiftingGroupYear year = await Send(new ManageUserGiftingGroupYearQuery(giftingGroupKey));
        var model = Mapper.Map<ManageUserGiftingGroupYearVm>(year);
        model.IncludePreviousYears = model.PreviousYearsRequired > 0 && model.OtherMembersSelect.Count > 0;

        if (model.Recipient != null)
        {
            model.RecipientSuggestions = await GetRecipientSuggestions(giftingGroupKey, model.Recipient.HashedUserId);
        }

        return View("Year", model);
    }

    [HttpGet]
    public async Task<IActionResult> RecipientSuggestionsGrid(int giftingGroupKey, string hashedUserId)
    {
        RecipientSuggestionsVm model = await GetRecipientSuggestions(giftingGroupKey, hashedUserId);
        return PartialView("_RecipientSuggestionsGrid", model);
    }

    private async Task<RecipientSuggestionsVm> GetRecipientSuggestions(int giftingGroupKey, string hashedUserId)
    {
        var suggestions = await Send(new GetRecipientSuggestionsQuery(giftingGroupKey, hashedUserId));
        return new RecipientSuggestionsVm(giftingGroupKey, hashedUserId, suggestions);
    }

    [HttpPost]
    public async Task<IActionResult> Year(ManageUserGiftingGroupYearVm model)
    {
        bool fromRadioButtons = model.SubmitIncludedChangeImmediately;
        var commandResult = await Send(new ParticipateInYearCommand<ManageUserGiftingGroupYearVm>(model), new UserGiftingGroupYearVmValidator());

        if (commandResult.Success)
        {
            string changed = model.Included ? "Included" : "Excluded";
            string message = $"{changed} Successfully";

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
