using Application.Areas.Messages.Commands;
using Application.Areas.Messages.Queries;
using Global.Abstractions.Global.Messages;
using Global.Extensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using ViewLayer.Models.Messages;

namespace Web.Areas.Messages.Controllers;

[Area("Messages")]
[Authorize]
public class HomeController : BaseController
{
    public HomeController(IServiceProvider services, SignInManager<IdentityUser> signInManager) : base(services, signInManager)
    {
    }

    public async Task<IActionResult> Index()
    {
        if (AjaxRequest())
            return await MessagesGrid();

        IQueryable<IReadMessage> messages = await Send(new GetMessagesQuery());
        return View(messages);
    }

    [HttpGet]
    public async Task<IActionResult> MessagesGrid()
    {
        IQueryable<IReadMessage> messages = await Send(new GetMessagesQuery());
        return PartialView("_MessagesGrid", messages);
    }

    [HttpGet]
    public async Task<IActionResult> ViewMessage(int messageRecipientKey)
    {
        try
        {
            IReadMessage message = await Send(new ViewMessageQuery(messageRecipientKey));
            var model = Mapper.Map<ReadMessageVm>(message);
            return PartialView("_ViewMessageModal", model);
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> MarkMessageRead(int messageRecipientKey)
    {
        try
        {
            await Send(new MarkMessageReadCommand(messageRecipientKey), null);
        }
        catch
        {
            // no point throwing an exception here
        }

        return Ok();
    }
}
