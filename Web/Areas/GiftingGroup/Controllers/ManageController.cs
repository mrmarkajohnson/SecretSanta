using Application.Santa.Areas.GiftingGroup.Actions;
using Application.Santa.Areas.GiftingGroup.Commands;
using Application.Santa.Areas.GiftingGroup.Queries;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models.GiftingGroup;
using Web.Controllers;
using static ViewLayer.Models.GiftingGroup.JoinGiftingGroupVm;

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
        ModelState.Clear();

        string saved = model.Id > 0 ? "Created" : "Updated";

        var commandResult = await Send(new SaveGiftingGroupCommand<EditGiftingGroupVm>(model), new EditGiftingGroupVmValidator());
        
        if (commandResult.Success)
        {
            return RedirectWithMessage(model, $"Gifting Group {saved} Successfully");
        }

        model.SubmitButtonText = model.Id > 0 ? "Create" : "Save Changes";
        return EditGiftingGroup(model);
    }

    [HttpGet]
    public IActionResult JoinGiftingGroup()
    {
        var model = new JoinGiftingGroupVm
        {
            GetGroupDetailsAction = nameof(GetGroupDetailsForJoiner)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> GetGroupDetailsForJoiner(JoinGiftingGroupVm model) // TODO: Call this
    {
        ModelState.Clear();
        
        bool found = await Send(new AddGroupDetailsForJoinerAction(model));

        if (model.Blocked)
        {
            ModelState.AddModelError(string.Empty, "You are blocked from applying to join this group. Please stop sending applications.");            
        }

        model.GetGroupDetailsAction = nameof(GetGroupDetailsForJoiner);
        return PartialView("_JoinGiftingGroup", model);
    }

    [HttpPost]
    public async Task<IActionResult> JoinGiftingGroup(JoinGiftingGroupVm model)
    {
        ModelState.Clear();

        var commandResult = await Send(new JoinGiftingGroupCommand<JoinGiftingGroupVm>(model), new JoinGiftingGroupVmValidator());

        if (commandResult.Success)
        {
            return RedirectWithMessage(model, $"Application sent. A group administrator will check your details and allow you to join.");
        }

        model.GetGroupDetailsAction = nameof(GetGroupDetailsForJoiner);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> JoinerApplications()
    {
        IQueryable<IReviewApplication> model = await Send(new GetJoinerRequestsQuery());
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ReviewJoinerApplication(int id)
    {
        IReviewApplication application = await Send(new ReviewJoinerApplicationQuery(id));
        var model = new ReviewJoinerApplicationVm();
        Mapper.Map(application, model);
        return View(model);
    }
}
