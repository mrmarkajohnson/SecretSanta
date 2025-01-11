using Application.Areas.Messages.Queries;
using Global.Abstractions.Global.Messages;

namespace Web.Areas.Messages.Controllers;

[Area("Messages")]
public class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index()
    {
        IQueryable<IReadMessage> messages = await Send(new GetMessagesQuery());
        return View(messages);
    }
}
