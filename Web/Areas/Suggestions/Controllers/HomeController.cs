using Application.Areas.Suggestions.Queries;
using ViewLayer.Models.Suggestions;

namespace Web.Areas.Suggestions.Controllers;

[Area("Suggestions")]
public class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    //public IActionResult Index()
    //{
    //    return View();
    //}

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
        return View("ManageSuggestion", model);
    }

    //[HttpPost]
    //public async Task<IActionResult> SaveSuggestion(ManageSuggestionVm model)
    //{

    //}
}
