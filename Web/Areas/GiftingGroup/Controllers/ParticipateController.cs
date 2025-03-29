using Application.Areas.GiftingGroup.Commands;
using Application.Areas.GiftingGroup.Queries;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using ViewLayer.Models.Participate;

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
    public async Task<IActionResult> Year(int groupId)
    {
        return await EditYearParticipation(groupId);
    }

    private async Task<IActionResult> EditYearParticipation(int groupId)
    {
        var year = await Send(new ManageUserGiftingGroupYearQuery(groupId));
        var model = Mapper.Map<ManageUserGiftingGroupYearVm>(year);
        model.IncludePreviousYears = model.PreviousYearsRequired > 0 && model.OtherMembersSelect.Count > 0;
        return View("Year", model);
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
                return await EditYearParticipation(model.GiftingGroupId);
            }
        }
        else
        {
            return FirstValidationError(commandResult);
        }

        //return View("Year", model);
    }
}
