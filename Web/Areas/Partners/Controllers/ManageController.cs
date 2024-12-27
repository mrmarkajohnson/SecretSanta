using Application.Santa.Areas.Partners.Queries;
using Global.Abstractions.Global.Partners;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ViewLayer.Models.Partners;
using Web.Controllers;

namespace Web.Areas.Partners.Controllers;

[Area("Partners")]
public class ManageController : BaseController
{
    public ManageController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index()
    {
        IRelationships relationships = await Send(new GetRelationshipsQuery());
        var model = Mapper.Map<RelationshipsVm>(relationships);
        return View(model);
    }

    // TODO: Save changes - make a button appear when changes are made
    // TODO: Add a new relationship, either from the menu or from the Index page
}
