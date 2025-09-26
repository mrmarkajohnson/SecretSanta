using Application.Areas.GiftingGroup.Actions;
using Application.Areas.GiftingGroup.BaseModels;
using Application.Areas.GiftingGroup.Commands;
using Application.Areas.GiftingGroup.Queries;
using Application.Areas.GiftingGroup.ViewModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.Intrinsics.Arm;
using static Global.Settings.GlobalSettings;

namespace Web.Areas.GiftingGroup.Controllers;

[Area(AreaNames.GiftingGroup)]
[Authorize]
public sealed class ManageController : BaseController
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

        return await ShowEditGiftingGroup(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditGiftingGroup(int giftingGroupKey)
    {
        var model = new EditGiftingGroupVm
        {
            SubmitButtonText = "Save Changes"
        };

        var groupDetails = await Send(new EditGiftingGroupQuery(giftingGroupKey));

        if (groupDetails != null)
        {
            Mapper.Map(groupDetails, model);
        }

        return await ShowEditGiftingGroup(model);
    }

    private async Task<IActionResult> ShowEditGiftingGroup(EditGiftingGroupVm model)
    {
        model.OtherGroupMembers = await GetOtherGroupMembers(model.GiftingGroupKey);
        return View("EditGiftingGroup", model);
    }

    private async Task<IEnumerable<IGroupMember>> GetOtherGroupMembers(int giftingGroupKey)
    {
        if (giftingGroupKey > 0)
        {
            return await Send(new GetGiftingGroupMembersQuery(giftingGroupKey, true));
        }
        else
        {
            return new List<IGroupMember>();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditGiftingGroup(EditGiftingGroupVm model)
    {
        ModelState.Clear();

        string saved = model.GiftingGroupKey > 0 ? "updated" : "created";

        var commandResult = await Send(new SaveGiftingGroupCommand<EditGiftingGroupVm>(model), new EditGiftingGroupVmValidator());

        if (commandResult.Success)
        {
            return RedirectWithMessage(model, $"Gifting group {saved} successfully");
        }

        model.SubmitButtonText = model.GiftingGroupKey > 0 ? "Save Changes" : "Create";
        return await ShowEditGiftingGroup(model);
    }

    [HttpGet]
    public async Task<IActionResult> GroupMembersGrid(int giftingGroupKey)
    {
        var model = new EditGiftingGroupVm
        {
            GiftingGroupKey = giftingGroupKey
        };

        model.OtherGroupMembers = await GetOtherGroupMembers(giftingGroupKey);
        return PartialView("_GiftingGroupMembersGrid", model);
    }

    [HttpGet]
    public async Task<IActionResult> SendGroupInvitation(int giftingGroupKey)
    {
        string groupName = HomeModel.GiftingGroups
            .Where(x => x.GroupAdmin)
            .FirstOrDefault(x => x.GiftingGroupKey == giftingGroupKey)?.GroupName
                ?? throw new AccessDeniedException("You do not have administrator access to this group.");

        var model = new SendGroupInvitationVm
        {
            GiftingGroupKey = giftingGroupKey,
            GiftingGroupName = groupName
        };

        await AddOtherGroupMembers(model);
        return PartialView("_SendInvitationModal", model);
    }

    private async Task AddOtherGroupMembers(SendGroupInvitationVm model)
    {
        var otherGroups = HomeModel.GiftingGroups
            .Where(x => x.GiftingGroupKey != model.GiftingGroupKey);

        if (otherGroups.Any())
        {
            model.OtherGroupMembers = await Send(new GetPossibleInviteesQuery(model.GiftingGroupKey));
        }
    }

    [HttpPost]
    public async Task<IActionResult> SendGroupInvitation(SendGroupInvitationVm model)
    {
        ModelState.Clear();

        var commandResult = await Send(new SendInvitationCommand<SendGroupInvitationVm>(model), new SendGroupInvitationVmValidator());

        if (commandResult.Success)
        {
            return Ok("Invitation sent successfully");
        }

        await AddOtherGroupMembers(model);
        return PartialView("_SendInvitationModal", model);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveGroupUser(int giftingGroupKey, int santaUserKey)
    {
        var model = new ChangeGroupMemberStatus(giftingGroupKey, santaUserKey);
        await Send(new RemoveUserFromGroupCommand(model), null);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> ToggleGroupAdmin(int giftingGroupKey, int santaUserKey)
    {
        var model = new ChangeGroupMemberStatus(giftingGroupKey, santaUserKey);
        await Send(new ToggleUserAdminStatusCommand(model), null);
        return Ok();
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetGroupDetailsForJoiner(JoinGiftingGroupVm model)
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
    //[ValidateAntiForgeryToken] // this has some problems due to using GetGroupDetailsForJoiner with submitFormViaFetch
    public async Task<IActionResult> JoinGiftingGroup(JoinGiftingGroupVm model)
    {
        ModelState.Clear();

        string joinerApplicationsUrl = GetFullUrl(nameof(JoinerApplications), nameof(ManageController), AreaNames.GiftingGroup);
        var commandResult = await Send(new JoinGiftingGroupCommand<JoinGiftingGroupVm>(model, joinerApplicationsUrl),
            new JoinGiftingGroupVmValidator());

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
        if (AjaxRequest())
            return await JoinerApplicationsGrid();

        var applications = await Send(new GetJoinerRequestsQuery());

        if (applications.Count() == 1)
            return RedirectToLocalUrl(Url.Action(nameof(ReviewJoinerApplication),
                new { groupApplicationKey = applications.First().GroupApplicationKey, singleApplication = true }));

        var model = new JoinerApplicationsVm
        {
            Applications = applications
        };

        return View(model);
    }

    public async Task<IActionResult> JoinerApplicationsGrid()
    {
        var model = new JoinerApplicationsVm();
        model.Applications = await Send(new GetJoinerRequestsQuery());
        return PartialView("_JoinerApplicationsGrid", model);
    }

    [HttpGet]
    public async Task<IActionResult> ReviewJoinerApplication(int groupApplicationKey, bool singleApplication = true)
    {
        IReviewApplication application = await Send(new ReviewJoinerApplicationQuery(groupApplicationKey));

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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReviewJoinerApplication(ReviewJoinerApplicationVm model)
    {
        string participateUrl = GetParticipateUrl();

        var commandResult = await Send(new ReviewJoinerApplicationCommand<ReviewJoinerApplicationVm>(model, participateUrl),
            new ReviewJoinerApplicationVmValidator());

        if (commandResult.Success)
        {
            if (model.SingleApplication && model.ReturnUrl?.Contains(nameof(JoinerApplications)) == true)
            {
                model.ReturnUrl = string.Empty;
            }

            string processed = model.Accepted ? "accepted" : "rejected";
            return RedirectWithMessage(model, $"Application {processed} successfully.");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> SetupGiftingGroupYear(int giftingGroupKey)
    {
        IGiftingGroupYear giftingGroupYear = await Send(new SetupGiftingGroupYearQuery(giftingGroupKey));
        var model = Mapper.Map<SetupGiftingGroupYearVm>(giftingGroupYear);

        if (model.RecalculationRequired)
        {
            model.Calculate = true;
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetupGiftingGroupYear(SetupGiftingGroupYearVm model, int giftingGroupKey) // giftingGroupKey is not used, but preserves the URL
    {
        var commandResult = await Send(new SetupGiftingGroupYearCommand<SetupGiftingGroupYearVm>(model), new SetupGiftingGroupYearVmValidator());

        if (commandResult.Success)
        {
            return RedirectWithMessage(model, $"Saved successfully");
        }

        return View(model);
    }
}
