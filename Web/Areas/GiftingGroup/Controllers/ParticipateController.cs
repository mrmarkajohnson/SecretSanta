using Application.Areas.GiftingGroup.Commands;
using Application.Areas.GiftingGroup.Queries;
using Microsoft.AspNetCore.Authorization;
using ViewLayer.Models.GiftingGroup;

namespace Web.Areas.GiftingGroup.Controllers;

[Area("GiftingGroup")]
[Authorize]
public class ParticipateController : BaseController
{
    public ParticipateController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index()
    {
        var groups = await Send(new UserGiftingGroupYearsQuery());
        return View(groups);
    }

    [HttpGet]
    public async Task<IActionResult> Year(int groupId)
    {
        var year = await Send(new GetUserGiftingGroupYearQuery(groupId));
        var model = Mapper.Map<UserGiftingGroupYearVm>(year);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ParticipateInYear(UserGiftingGroupYearVm model)
    {
        var commandResult = await Send(new ParticipateInYearCommand<UserGiftingGroupYearVm>(model), new UserGiftingGroupYearVmValidator());

        if (commandResult.Success && !string.IsNullOrEmpty(model.ReturnUrl))
        {
            string changed = model.Included ? "Included" : "Excluded";
            return RedirectWithMessage(model.ReturnUrl, $"{changed} Successfully");
        }

        return View("Year", model);
    }
}
