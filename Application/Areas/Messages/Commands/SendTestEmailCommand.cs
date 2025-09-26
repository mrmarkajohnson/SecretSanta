using Application.Areas.Messages.BaseModels;
using Application.Areas.Messages.ViewModels;
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

        if (DbContext.EmailClient == null)
            throw new NotFoundException("The e-mail client has not been configured.");

        var message = new SantaMessage
        {
            HeaderText = StandardPhrases.TestEmailHeader,
            MessageText = "This is a test e-mail from Secret Santa.",
            ShowAsFromSanta = true
        };

        var recipient = new EmailRecipient
        {
            Forename = "Test Recipient",
            Surname="Recipient",
            Email = Item.RecipientEmailAddress,
            IdentificationHashed = false,
            EmailConfirmed = true,
            ReceiveEmails = MessageSettings.EmailPreference.All,
            DetailedEmails = true,
            Greeting = "Hello there"
        };

        try
        {
            Validation = DbContext.EmailClient.SendMessage(message, [recipient]);
        }
        catch (Exception ex)
        {
            Validation.AddError(ex.Message);
        }

        return await SuccessResult();
    }
}
