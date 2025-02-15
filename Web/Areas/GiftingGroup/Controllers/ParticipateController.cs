using Application.Areas.GiftingGroup.Commands;
using Application.Areas.GiftingGroup.Queries;
using Microsoft.AspNetCore.Authorization;
using ViewLayer.Models.Participate;

namespace Web.Areas.GiftingGroup.Controllers;

[Area("GiftingGroup")]
[Authorize]
public class ParticipateController : BaseController
{
    public ParticipateController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GiftingGroupsGrid()
    {
        var groups = await Send(new UserGiftingGroupYearsQuery());
        return PartialView("_GiftingGroupsGrid", groups);
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

        if (commandResult.Success)
        {
            string changed = model.Included ? "Included" : "Excluded";
            string message = $"{changed} Successfully";

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return RedirectWithMessage(model.ReturnUrl, message);
            }
            else
            {
                return Ok(message);
            }
        }
        else
        {
            return FirstValidationError(commandResult);
        }

        //return View("Year", model);
    }
}
