using Application.Areas.Partners.Commands;
using Application.Areas.Partners.Queries;
using Global.Abstractions.Areas.Partners;
using Microsoft.AspNetCore.Authorization;
using ViewLayer.Models.Partners;
using ViewLayer.Models.Shared;
using static Global.Settings.PartnerSettings;

namespace Web.Areas.Partners.Controllers;

[Area("Partners")]
[Authorize]
public class ManageController : BaseController
{
    public ManageController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index(string? successMessage = null)
    {
        IRelationships relationships = await Send(new GetRelationshipsQuery());
        var model = Mapper.Map<RelationshipsVm>(relationships);
        model.SuccessMessage = successMessage;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRelationshipStatus(int partnerLinkKey, Guid globalUserId, RelationshipStatus newStatus)
    {
        string manageRelationshipsLink = GetFullUrl(nameof(Index), "Manage", "Partners");
        var model = new ChangeRelationshipStatusVm(partnerLinkKey, globalUserId, newStatus, manageRelationshipsLink);
        var result = await Send(new ChangeRelationshipStatusCommand(model), null);

        if (result.Success)
        {
            if (model.NewStatus == RelationshipStatus.NotRelationship)
                return RedirectWithMessage(Url.Action(nameof(Index), "Manage", new { Area = "Partners" }), "Relationship cancelled successfully");

            return Ok("Relationship updated successfully");
        }
        else
        {
            return FirstValidationError(result);
        }
    }

    [HttpGet]
    public async Task<IActionResult> AddRelationship()
    {
        if (AjaxRequest())
            return await SelectRelationshipUserGrid();

        AddRelationshipVm model = await GetAddRelationshipModel();
        return View(model);
    }

    private async Task<AddRelationshipVm> GetAddRelationshipModel(Guid? globalUserId = null)
    {
        var possiblePartners = await Send(new GetPossiblePartnersQuery());
        string manageRelationshipsLink = GetFullUrl(nameof(Index), "Manage", "Partners");

        var model = new AddRelationshipVm(possiblePartners, manageRelationshipsLink, nameof(SelectRelationshipUserGrid));
        model.GlobalUserId = globalUserId ?? Guid.Empty;

        return model;
    }

    public async Task<IActionResult> SelectRelationshipUserGrid()
    {
        var possiblePartners = await Send(new GetPossiblePartnersQuery());
        var model = new UserGridVm(possiblePartners, nameof(SelectRelationshipUserGrid));
        return PartialView("_SelectUserGrid", model);
    }

    // TODO: Allow deleting relationships if they haven't been confirmed yet

    [HttpPost]
    public async Task<IActionResult> AddRelationship(Guid globalUserId)
    {
        var model = await GetAddRelationshipModel(globalUserId);
        var result = await Send(new AddRelationshipCommand(model), null);

        if (result.Success)
        {
            return RedirectWithMessage(Url.Action(nameof(Index), "Manage", new { Area = "Partners" }), "Relationship added successfully");
        }
        else
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, result.Validation.Errors[0].ErrorMessage);
        }
    }
}
