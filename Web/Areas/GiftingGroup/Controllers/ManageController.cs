using Application.Santa.Areas.GiftingGroup.Commands;
using Application.Santa.Areas.GiftingGroup.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models.GiftingGroup;
using Web.Controllers;

namespace Web.Areas.GiftingGroup.Controllers;

[Area("GiftingGroup")]
[Authorize]
public class ManageController : BaseController
{
    public ManageController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> CreateGiftingGroup()
    {
        var model = new EditGiftingGroupVm
        {
            SubmitButtonText = "Create",
            JoinerToken = await Send(new GetUniqueJoinerTokenQuery())
        };

        return EditGiftingGroup(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditGiftingGroup(int groupId)
    {
        var model = new EditGiftingGroupVm
        {
            SubmitButtonText = "Save Changes"
        };

        var groupDetails = await Send(new EditGiftingGroupQuery(groupId));

        if (groupDetails != null)
        {
            Mapper.Map(groupDetails, model);
        }

        return EditGiftingGroup(model);
    }

    private IActionResult EditGiftingGroup(EditGiftingGroupVm model)
    {
        return View("EditGiftingGroup", model);
    }

    [HttpPost]
    public async Task<IActionResult> SaveGiftingGroup(EditGiftingGroupVm model)
    {
        string saved = model.Id > 0 ? "Created" : "Updated";

        var commandResult = await Send(new SaveGiftingGroupCommand<EditGiftingGroupVm>(model), new EditGiftingGroupVmValidator());
        
        if (commandResult.Success)
        {
            model.ReturnUrl ??= Url.Content("~/");
            return RedirectWithMessage(model, $"Gifting Group {saved} Successfully");
        }

        model.SubmitButtonText = model.Id > 0 ? "Create" : "Save Changes";
        return EditGiftingGroup(model);
    }
}
