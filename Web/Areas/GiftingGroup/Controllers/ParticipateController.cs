using Application.Areas.GiftingGroup.Queries;
using Microsoft.AspNetCore.Authorization;

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
        var groups = await Send(new UserGiftingGroupYearsQuery());
        return View(groups);
    }

    public IActionResult Year(int groupId)
    {
        return View();
    }
}
