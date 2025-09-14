using Application.Areas.Suggestions.Commands;
using Application.Areas.Suggestions.Queries;
using Application.Areas.Suggestions.ViewModels;
using Global.Abstractions.Areas.Suggestions;
using Microsoft.AspNetCore.Authorization;
using Web.Areas.GiftingGroup.Controllers;
using static Global.Settings.GlobalSettings;

namespace Web.Areas.Suggestions.Controllers;

[Area(AreaNames.Suggestions)]
[Authorize]
public class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index()
    {
        MySuggestionsVm model = await GetMySuggestionsVm();
        return View(model);
    }

    private async Task<MySuggestionsVm> GetMySuggestionsVm()
    {
        IQueryable<ISuggestion> suggestions = await Send(new GetCurrentUserSuggestionsQuery());
        var model = new MySuggestionsVm(suggestions, HomeModel.GiftingGroups);
        return model;
    }

    [HttpGet]
    public async Task<IActionResult> MySuggestionsGrid()
    {
        MySuggestionsVm model = await GetMySuggestionsVm();
        return PartialView("_MySuggestionsGrid", model);
    }

    [HttpGet]
    public async Task<IActionResult> AddSuggestion(int? giftingGroupKey = null)
    {
        var suggestion = await Send(new ManageSuggestionQuery(null, giftingGroupKey));
        var model = Mapper.Map<ManageSuggestionVm>(suggestion);
        return View("ManageSuggestion", model);
    }

    [HttpGet]
    public async Task<IActionResult> EditSuggestion(int suggestionKey)
    {
        var suggestion = await Send(new ManageSuggestionQuery(suggestionKey, null));
        var model = Mapper.Map<ManageSuggestionVm>(suggestion);
        model.IsModal = true;
        return PartialView("_ManageSuggestionModal", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSuggestion(ManageSuggestionVm model)
    {
        ModelState.Clear();
        bool update = model.SuggestionKey > 0;

        var yearGroupUrl = GetFullUrl(nameof(ParticipateController.Year), nameof(ParticipateController), AreaNames.GiftingGroup, new { giftingGroupKey = 0 });
        var commandResult = await Send(new SaveSuggestionCommand<ManageSuggestionVm>(model, yearGroupUrl), new ManageSuggestionVmValidator());

        if (commandResult.Success)
        {
            string changed = update ? "updated" : "added";
            string message = $"Suggestion {changed} successfully";

            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                if (model.IsModal)
                {
                    return Ok(message);
                }

                model.ReturnUrl = Url.Action(nameof(Index));
            }

            return RedirectWithMessage(model.ReturnUrl, message);
        }
        else if (model.IsModal)
        {
            return PartialView("_ManageSuggestionModal", model);
        }
        else
        {
            return View("ManageSuggestion", model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSuggestion(int suggestionKey)
    {
        var commandResult = await Send(new DeleteSuggestionCommand(suggestionKey), null);

        if (commandResult.Success)
        {
            return Ok("Suggestion deleted succesfully");
        }
        else
        {
            return FirstValidationError(commandResult);
        }
    }
}
