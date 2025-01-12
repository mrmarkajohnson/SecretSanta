using Application.Areas.Messages.Queries;
using Global.Abstractions.Global.Messages;
using Global.Extensions.Exceptions;

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

    public async Task<IActionResult> ViewMessage(int id)
    {
        try
        {
            IReadMessage message = await Send(new ViewMessageQuery(id));
            return PartialView("_ViewMessageModal", message);
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
        }
    }
}
