using Application.Areas.Partners.Commands;
using Application.Areas.Partners.Queries;
using Global.Abstractions.Areas.Partners;
using Global.Extensions.Exceptions;
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
            if (model.NewStatus == RelationshipStatus.IgnoreNonRelationship)
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
    public async Task<IActionResult> AddRelationship(Guid globalUserId, bool active)
    {
        var model = await GetAddRelationshipModel(globalUserId);
        model.IsActive = active;

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

    [HttpGet]
    public async Task<IActionResult> EditRelationship(int partnerLinkKey, Guid globalUserId)
    {
        IRelationships relationships = await Send(new GetRelationshipsQuery());
        var relationship = relationships.PossibleRelationships
            .FirstOrDefault(x => x.PartnerLinkKey == partnerLinkKey && x.Partner.GlobalUserId == globalUserId.ToString());

        if (relationship == null)
            return ErrorMessageResult("Relationship not found.");

        var model = Mapper.Map<ManageRelationshipVm>(relationship);
        return PartialView("_ManageRelationshipModal", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditRelationship(ManageRelationshipVm model)
    {
        ModelState.Clear();

        bool isValid = ValidateItem(model, new ManageRelationshipVmValidator());

        if (isValid)
        {
            string manageRelationshipsLink = GetFullUrl(nameof(Index), "Manage", "Partners");
            var changeModel = new ChangeRelationshipStatusVm(model.PartnerLinkKey ?? 0, new Guid(model.Partner.GlobalUserId), model.Status, manageRelationshipsLink);
            var result = await Send(new ChangeRelationshipStatusCommand(changeModel), null);

            if (result.Success)
            {
                return Ok("Relationship updated successfully");
            }
        }

        return PartialView("_ManageRelationshipModal", model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRelationship(int partnerLinkKey, Guid globalUserId)
    {
        string manageRelationshipsLink = GetFullUrl(nameof(Index), "Manage", "Partners");
        var model = new ChangeRelationshipStatusVm(partnerLinkKey, globalUserId, RelationshipStatus.EndedBeforeConfirmation, manageRelationshipsLink);
        var result = await Send(new ChangeRelationshipStatusCommand(model), null);

        if (result.Success)
        {
            if (model.NewStatus == RelationshipStatus.IgnoreNonRelationship)
                return RedirectWithMessage(Url.Action(nameof(Index), "Manage", new { Area = "Partners" }), "Relationship cancelled successfully");

            return Ok("Relationship deleted successfully");
        }
        else
        {
            return FirstValidationError(result);
        }
    }
}
