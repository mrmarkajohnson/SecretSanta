using Application.Areas.Messages.BaseModels;
using Application.Areas.Messages.ViewModels;
using Application.Shared.BaseModels;
using Application.Shared.Requests;
using FluentValidation;
using Global.Extensions.Exceptions;

namespace Application.Areas.Messages.Commands;

public sealed class SendTestEmailCommand : BaseCommand<SendTestEmailVm>
{
    public SendTestEmailCommand(SendTestEmailVm item) : base(item)
    {
    }

    protected async override Task<ICommandResult<SendTestEmailVm>> HandlePostValidation()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser);

        if (!dbCurrentUser.SystemAdmin)
            throw new AccessDeniedException("Only system administrators can send test e-mails.");

        if (EmailClient == null)
            throw new NotFoundException("The e-mail client has not been configured.");

        var message = new MessageBase
        {
            HeaderText = "Test Email",
            MessageText = "This is a test e-mail from Secret Santa."
        };

        var recipient = new UserNamesBase
        {
            Forename = "Test",
            Surname="Recipient",
            Email = Item.RecipientEmailAddress,
            IdentificationHashed = false
        };

        try
        {
            Validation = EmailClient.SendMessage(message, [recipient]);
        }
        catch (Exception ex)
        {
            Validation.AddError(ex.Message);
        }

        return await SuccessResult();
    }
}
