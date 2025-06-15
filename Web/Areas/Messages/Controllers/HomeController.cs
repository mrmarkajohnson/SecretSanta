using Application.Areas.Messages.Commands;
using Application.Areas.Messages.Queries;
using Application.Areas.Messages.ViewModels;
using Global.Abstractions.Areas.Messages;
using Global.Extensions.Exceptions;
using Global.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Web.Areas.Messages.Controllers;

[Area("Messages")]
[Authorize]
public sealed class HomeController : BaseController
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

    [HttpGet]
    public IActionResult WriteMessage(int? giftingGroupKey)
    {
        var model = new SendMessageVm
        {
            GiftingGroupKey = giftingGroupKey
        };

        var giftingGroups = HomeModel.GiftingGroups;
        model.GiftingGroups = giftingGroups;

        if (giftingGroupKey > 0 && !giftingGroups.Any(x => x.GiftingGroupKey == giftingGroupKey.Value))
        {
            model.GiftingGroupKey = giftingGroupKey = null;
        }
        
        if (giftingGroupKey.IsEmpty() && giftingGroups.Count == 1)
        {
            model.GiftingGroupKey = giftingGroups.First().GiftingGroupKey;
        }

        return View(model);
    }

    // TODO: Add message reply option; include the original message type in the model

    [HttpPost]
    public async Task<IActionResult> SendMessage(SendMessageVm model)
    {
        model.RecipientType = model.RecipientType.ActualType(model.IncludeFutureMembers);

        throw new NotImplementedException();
    }
}
