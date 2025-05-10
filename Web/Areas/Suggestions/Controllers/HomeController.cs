using Application.Areas.Suggestions.Commands;
using Application.Areas.Suggestions.Queries;
using Global.Abstractions.Areas.Suggestions;
using ViewLayer.Models.Suggestions;

namespace Web.Areas.Suggestions.Controllers;

[Area("Suggestions")]
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
    public async Task<IActionResult> SaveSuggestion(ManageSuggestionVm model)
    {
        ModelState.Clear();
        bool update = model.SuggestionKey > 0;
        
        var commandResult = await Send(new SaveSuggestionCommand<ManageSuggestionVm>(model), new ManageSuggestionVmValidator());

        if (commandResult.Success)
        {
            string changed = update ? "Updated" : "Added";
            string message = $"Suggestion {changed} Successfully";

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
