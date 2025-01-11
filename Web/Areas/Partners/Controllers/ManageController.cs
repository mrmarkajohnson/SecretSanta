using Application.Areas.Partners.Commands;
using Application.Areas.Partners.Queries;
using Global.Abstractions.Areas.Partners;
using ViewLayer.Models.Partners;
using static Global.Settings.PartnerSettings;

namespace Web.Areas.Partners.Controllers;

[Area("Partners")]
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
    public async Task<IActionResult> ChangeRelationshipStatus(int partnerLinkId, Guid userId, RelationshipStatus newStatus)
    {
        string manageRelationshipsLink = GetFullUrl(nameof(Index), nameof(ManageController), "Partners");
        var model = new ChangeRelationshipStatusVm(partnerLinkId, userId, newStatus, manageRelationshipsLink);
        var result = await Send(new ChangeRelationshipStatusCommand(model), null);

        if (result.Success)
        {
            if (model.NewStatus == RelationshipStatus.NotRelationship)
                return RedirectWithMessage(Url.Action(nameof(Index), nameof(ManageController), new { Area = "Partners" }), "Relationship cancelled successfully");

            return Ok("Relationship updated successfully");
        }
        else
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, result.Validation.Errors[0].ErrorMessage);
        }
    }

    [HttpGet]
    public async Task<IActionResult> AddRelationship()
    {
        AddRelationshipVm model = await GetAddRelationshipModel();
        return View(model);
    }

    private async Task<AddRelationshipVm> GetAddRelationshipModel(Guid? userId = null)
    {
        var possiblePartners = await Send(new GetPossiblePartnersQuery());
        string manageRelationshipsLink = GetFullUrl(nameof(Index), nameof(ManageController), "Partners");

        var model = new AddRelationshipVm(possiblePartners, manageRelationshipsLink);
        model.UserId = userId ?? Guid.Empty;

        return model;
    }

    // TODO: Allow deleting relationships if they haven't been confirmed yet

    [HttpPost]
    public async Task<IActionResult> AddRelationship(Guid userId)
    {
        var model = await GetAddRelationshipModel(userId);
        var result = await Send(new AddRelationshipCommand(model), null);

        if (result.Success)
        {
            return RedirectWithMessage(Url.Action(nameof(Index), nameof(ManageController), new { Area = "Partners" }), "Relationship added successfully");
        }
        else
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, result.Validation.Errors[0].ErrorMessage);
        }
    }
}
