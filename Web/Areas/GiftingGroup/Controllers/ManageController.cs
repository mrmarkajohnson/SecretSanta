using Application.Santa.Areas.GiftingGroup;
using Application.Santa.Areas.GiftingGroup.Commands;
using Application.Santa.Areas.GiftingGroup.Queries;
using Global.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models.GiftingGroup;
using Web.Controllers;

namespace Web.Areas.GiftingGroup.Controllers;

[Area("GiftingGroup")]
public class ManageController : BaseController
{
    public ManageController(IServiceProvider services, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : base(services, userManager, signInManager)
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

        var groupDetails = await Send(new EditGiftingGroupQuery(groupId, User, UserManager, SignInManager));

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

        var commandResult = await Send(new SaveGiftingGroupCommand<EditGiftingGroupVm>(model, User, UserManager, SignInManager), new EditGiftingGroupVmValidator());
        
        if (commandResult.Success)
        {
            model.ReturnUrl ??= Url.Content("~/");
            return RedirectWithMessage(model, $"Gifting Group {saved} Successfully");
        }

        model.SubmitButtonText = model.Id > 0 ? "Create" : "Save Changes";
        return EditGiftingGroup(model);
    }
}
