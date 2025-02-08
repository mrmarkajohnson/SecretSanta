using Application.Areas.GiftingGroup.Actions;
using Application.Areas.GiftingGroup.Commands;
using Application.Areas.GiftingGroup.Queries;
using Global.Abstractions.Areas.GiftingGroup;
using Microsoft.AspNetCore.Authorization;
using ViewLayer.Models.GiftingGroup;
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

        return ShowEditGiftingGroup(model);
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

        return ShowEditGiftingGroup(model);
    }

    private IActionResult ShowEditGiftingGroup(EditGiftingGroupVm model)
    {
        return View("EditGiftingGroup", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditGiftingGroup(EditGiftingGroupVm model)
    {
        ModelState.Clear();

        string saved = model.Id > 0 ? "Updated" : "Created";

        var commandResult = await Send(new SaveGiftingGroupCommand<EditGiftingGroupVm>(model), new EditGiftingGroupVmValidator());
        
        if (commandResult.Success)
        {
            return RedirectWithMessage(model, $"Gifting Group {saved} Successfully");
        }

        model.SubmitButtonText = model.Id > 0 ? "Save Changes" : "Create" ;
        return ShowEditGiftingGroup(model);
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
        var model = new JoinerApplicationsVm();
        model.Applications = await Send(new GetJoinerRequestsQuery());

        if (model.Applications.Count() == 1)
            return LocalRedirect(Url.Action(nameof(ReviewJoinerApplication), 
                new { applicationId = model.Applications.First().ApplicationId, singleApplication = true }));

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ReviewJoinerApplication(int applicationId, bool singleApplication = true)
    {
        IReviewApplication application = await Send(new ReviewJoinerApplicationQuery(applicationId));
        
        var model = new ReviewJoinerApplicationVm();
        Mapper.Map(application, model);

        model.SingleApplication = singleApplication;
        if (!singleApplication)
        {
            model.ReturnUrl = Url.Action(nameof(JoinerApplications));
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ReviewJoinerApplication(ReviewJoinerApplicationVm model)
    {
        var commandResult = await Send(new ReviewJoinerApplicationCommand<ReviewJoinerApplicationVm>(model), new ReviewJoinerApplicationVmValidator());

        if (commandResult.Success)
        {
            if (model.SingleApplication && model.ReturnUrl?.Contains(nameof(JoinerApplications)) == true)
            {
                model.ReturnUrl = string.Empty;
            }
            
            string processed = model.Accepted ? "Accepted" : "Rejected";
            return RedirectWithMessage(model, $"Application {processed} Successfully");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> SetupGiftingGroupYear(int groupId)
    {
        IGiftingGroupYear giftingGroupYear = await Send(new SetupGiftingGroupYearQuery(groupId));
        var model = Mapper.Map<SetupGiftingGroupYearVm>(giftingGroupYear);

        if (model.RecalculationRequired)
        {
            model.Calculate = true;
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SetupGiftingGroupYear(SetupGiftingGroupYearVm model, int groupId) // groupId is not used, but preserves the URL
    {
        var commandResult = await Send(new SetupGiftingGroupYearCommand<SetupGiftingGroupYearVm>(model), new SetupGiftingGroupYearVmValidator());

        if (commandResult.Success)
        {
            return RedirectWithMessage(model, $"Saved Successfully");
        }

        return View(model);
    }
}
