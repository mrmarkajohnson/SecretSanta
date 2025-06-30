using Application.Areas.GiftingGroup.Queries;
using Application.Areas.Messages.Commands;
using Application.Areas.Messages.Queries;
using Application.Areas.Messages.ViewModels;
using Global.Abstractions.Areas.GiftingGroup;
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
    public async Task<IActionResult> ViewMessage(int messageKey, int? messageRecipientKey)
    {
        try
        {
            IReadMessage message = await Send(new ViewMessageQuery(messageKey, messageRecipientKey));
            var model = Mapper.Map<ReadMessageVm>(message);
            return PartialView("_ViewMessageModal", model);
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> MarkMessageRead(int messageKey, int? messageRecipientKey)
    {
        try
        {
            await Send(new MarkMessageReadCommand(messageKey, messageRecipientKey), null);
        }
        catch
        {
            // no point throwing an exception here
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> WriteMessage(int? giftingGroupKey)
    {
        WriteMessageVm model = await SendMessage(giftingGroupKey);
        model.ReturnUrl = Url.Action(nameof(Index));
        return View(model);
    }

    private async Task<WriteMessageVm> SendMessage(int? giftingGroupKey)
    {
        var model = new WriteMessageVm
        {
            GiftingGroupKey = giftingGroupKey
        };

        model.GiftingGroups = HomeModel.GiftingGroups;

        if (model.GiftingGroupKey > 0 && !model.GiftingGroups.Any(x => x.GiftingGroupKey == model.GiftingGroupKey.Value))
        {
            model.GiftingGroupKey = null;
        }

        if (model.GiftingGroupKey.IsEmpty() && model.GiftingGroups.Count == 1)
        {
            model.GiftingGroupKey = model.GiftingGroups.First().GiftingGroupKey;
        }

        model.GroupKeyPreset = model.GiftingGroupKey > 0;

        await AddGroupMembers(model);
        return model;
    }

    [HttpPost]
    public async Task<IActionResult> ChooseMessageRecipient(ChooseMessageRecipientVm model)
    {
        await AddGroupMembers(model);
        return PartialView("_ChooseMessageRecipient", model);
    }

    private async Task AddGroupMembers(ChooseMessageRecipientVm model)
    {
        if (model.GiftingGroupKey > 0)
        {
            var groupMembers = await Send(new GetGiftingGroupMembersQuery(model.GiftingGroupKey.Value));
            model.OtherGroupMembers = groupMembers.Where(x => x.SantaUserKey != HomeModel.CurrentUser?.SantaUserKey).ToList();
            
            model.GroupAdmin = groupMembers.FirstOrDefault(x => x.SantaUserKey == HomeModel.CurrentUser?.SantaUserKey)?.GroupAdmin == true;
            // TODO: Process group admins label if the current user is an admin, but there are also other admins
        }
        else
        {
            model.OtherGroupMembers = new List<IGroupMember>();
        }
    }

    // TODO: Add message reply option; include the original message type in the model

    [HttpPost]
    public async Task<IActionResult> WriteMessage(WriteMessageVm model)
    {
        return await SendMessage(model);
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(WriteMessageVm model)
    {
        ModelState.Clear();

        model.GiftingGroups = HomeModel.GiftingGroups;
        await AddGroupMembers(model);
        
        var commandResult = await Send(new WriteMessageCommand<WriteMessageVm>(model), new WriteMessageVmValidator());

        if (commandResult.Success)
        {
            string message = "Message sent successfully";

            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                if (model.IsModal)
                {
                    return Ok(message);
                }

                model.ReturnUrl = Url.Action(nameof(Index));
            }

            return RedirectWithMessage(model.ReturnUrl, message);
        }
        else
        {
            model.SetDisplayRecipientType();
            
            if (model.IsModal)
            {
                return PartialView("_WriteMessageModal", model);
            }
            else
            {
                return View("WriteMessage", model);
            }
        }
    }
}
