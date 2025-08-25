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

        IQueryable<ISantaMessage> messages = await GetMessages();
        return View(messages);
    }

    [HttpGet]
    public async Task<IActionResult> MessagesGrid()
    {
        IQueryable<ISantaMessage> messages = await GetMessages();
        return PartialView("_MessagesGrid", messages);
    }

    private async Task<IQueryable<ISantaMessage>> GetMessages()
    {
        return await Send(new GetMessagesQuery());
    }

    public async Task<IActionResult> SentMessages()
    {
        if (AjaxRequest())
            return await SentMessagesGrid();

        IQueryable<ISentMessage> messages = await GetSentMessages();
        return View(messages);
    }

    [HttpGet]
    public async Task<IActionResult> SentMessagesGrid()
    {
        IQueryable<ISentMessage> messages = await GetSentMessages();
        return PartialView("_SentMessagesGrid", messages);
    }

    private async Task<IQueryable<ISentMessage>> GetSentMessages()
    {
        return await Send(new SentMessagesQuery());
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

    [HttpGet]
    public async Task<IActionResult> ViewSentMessage(int messageKey)
    {
        try
        {
            IReadMessage message = await Send(new ViewSentMessageQuery(messageKey));
            var model = Mapper.Map<ReadMessageVm>(message);
            model.IsSentMessage = true; // just in case
            return PartialView("_ViewMessageModal", model);
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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
        WriteMessageVm model = await GetSendMessageModel(giftingGroupKey);
        model.ReturnUrl = Url.Action(nameof(SentMessages));
        return View(model);
    }

    private async Task<WriteMessageVm> GetSendMessageModel(int? giftingGroupKey)
    {
        var model = new WriteMessageVm
        {
            GiftingGroupKey = giftingGroupKey,
            AddSuggestionUrl = GetFullUrl("AddSuggestion", "Home", "Suggestions"),
            GiftingGroups = HomeModel.GiftingGroups
        };

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
    [ValidateAntiForgeryToken] // sent via fetch
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

    [HttpGet]
    public async Task<IActionResult> Reply(int messageKey, int? messageRecipientKey)
    {
        try
        {
            var model = new WriteReplyVm();
            await SetUpReply(model, messageKey, messageRecipientKey);
            return PartialView("_WriteMessageModal", model);
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ReplyToSent(int messageKey)
    {
        try
        {
            var model = new WriteReplyVm();
            await SetUpReply(model, messageKey, true);
            return PartialView("_WriteMessageModal", model);
        }
        catch (NotFoundException ex)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
        }
    }

    private async Task SetUpReply(WriteMessageVm model, int replyToMessageKey, int? messageRecipientKey)
    {
        IReadMessage originalMessage = await Send(new ViewMessageQuery(replyToMessageKey, messageRecipientKey));
        SetUpReply(model, originalMessage, originalMessage.IsSentMessage);
    }

    private async Task SetUpReply(WriteMessageVm model, int replyToMessageKey, bool replyToSentMessage)
    {
        IReadMessage originalMessage = replyToSentMessage 
            ? await Send(new ViewSentMessageQuery(replyToMessageKey))
            : await Send(new ViewMessageQuery(replyToMessageKey));

        SetUpReply(model, originalMessage, replyToSentMessage || originalMessage.IsSentMessage);
    }

    private void SetUpReply(WriteMessageVm model, IReadMessage originalMessage, bool replyToSentMessage)
    {
        if (originalMessage != null)
        {
            model.ReplyToMessageKey = originalMessage.MessageKey;
            model.GiftingGroupKey = originalMessage.GiftingGroupKey;
            model.GroupName = originalMessage.GroupName;
            model.IsModal = true;
            model.ReturnUrl = Url.Action(nameof(SentMessages));
            model.OriginalRecipientType = originalMessage.RecipientType;
            model.ReplyToName = originalMessage.SenderName;

            if (string.IsNullOrWhiteSpace(model.HeaderText))
                model.HeaderText = "RE: " + originalMessage.HeaderText.TrimStart("RE: ");

            model.PreviousMessages = (new List<ISantaMessage> { originalMessage })
                .Union(originalMessage.PreviousMessages)
                .ToList();

            model.LaterMessages = originalMessage.LaterMessages;

            if (replyToSentMessage || originalMessage.IsSentMessage)
            {
                model.ReplyToName = originalMessage.SenderToDescription();
            }
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> WriteMessage(WriteMessageVm model)
    {
        return await SendMessage(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reply(WriteReplyVm model)
    {
        return await SendMessage(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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

                model.ReturnUrl = Url.Action(nameof(SentMessages));
            }

            return RedirectWithMessage(model.ReturnUrl, message);
        }
        else
        {
            return await SendMessageFailed(model);
        }
    }

    private async Task<IActionResult> SendMessageFailed(WriteMessageVm model)
    {
        if (model.ReplyToMessageKey > 0)
        {
            await SetUpReply(model, model.ReplyToMessageKey.Value, false);
        }

        model.SetDisplayRecipientType();
        model.AddSuggestionUrl = GetFullUrl("AddSuggestion", "Home", "Suggestions");        

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
