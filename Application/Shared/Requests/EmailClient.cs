using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using Global.Abstractions.Areas.Messages;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace Application.Shared.Requests;

internal class EmailClient : IEmailClient
{
    public EmailClient(IServiceProvider services)
    {
        _mailSettings = services.GetRequiredService<IMailSettings>();
        _mapper = services.GetRequiredService<IMapper>();
    }

    private readonly IMailSettings _mailSettings;
    private readonly IMapper _mapper;

    public ValidationResult SendMessage(Santa_Message dbMessage)
    {
        if (!dbMessage.Recipients.Any())
        {
            var result = new ValidationResult();
            result.AddError($"No recipients were found.");
            return result;
        }

        var recipients = dbMessage.Recipients
            .AsQueryable()
            .ProjectTo<IEmailRecipient>(_mapper.ConfigurationProvider)
            .ToList();

        recipients.ForEach(x => x.UnHash());
        return SendMessage(dbMessage, recipients);
    }

    public ValidationResult SendMessage(IMessageBase message, List<IEmailRecipient> recipients)
    {
        var result = new ValidationResult();

        List<IEmailRecipient> validRecipients = recipients.Where(x => x.Email.IsNotEmpty()).ToList();

        if (!validRecipients.Any())
        {
            result.AddWarning($"No valid recipients were found.");
            return result;
        }

        try
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_mailSettings.UserName, _mailSettings.Password);

                foreach (IEmailRecipient recipient in validRecipients)
                {
                    SendMessage(message, recipient, client, result);
                }

                client.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            result.AddError(ex.Message);
        }

        return result;
    }

    private void SendMessage(IMessageBase message, IEmailRecipient recipient, SmtpClient client, ValidationResult result)
    {
        if (recipient.IdentificationHashed)
            recipient.UnHash();

        if (string.IsNullOrEmpty(recipient.Email))
        {
            result.AddWarning($"{recipient.FullName()}'s e-mail address is empty.", nameof(recipient.Email));
            return;
        }

        if (!EmailHelper.IsEmail(recipient.Email))
        {
            result.AddWarning($"{recipient.FullName()}'s e-mail address is not valid.", nameof(recipient.Email));
            return;
        }

        string messageText = GetMessageText(message, recipient);

        try
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(_mailSettings.FromName, _mailSettings.FromAddress));
            mimeMessage.To.Add(new MailboxAddress(recipient.DisplayName(), recipient.Email));
            mimeMessage.Subject = message.HeaderText;
            mimeMessage.Body = new TextPart("html") { Text = messageText };

            client.Send(mimeMessage);
            return;
        }
        catch (Exception ex)
        {
            result.AddError($"Could not send e-mail to {recipient.FullName()}. {ex.Message}", nameof(recipient.Email));
        }
    }

    private string GetMessageText(IMessageBase message, IEmailRecipient recipient)
    {
        string? viewMessageUrl = MessageSettings.ViewMessageUrl.IsNotEmpty() && recipient.MessageKey > 0
            ? $"?messageKey={recipient.MessageKey}&messageRecipientKey={recipient.MessageRecipientKey}"
            : null;

        string messageText = message.MessageText + $"<br/><br/>You cannot reply directly to this e-mail.";

        if (viewMessageUrl.IsNotEmpty())
        {
            messageText += $" Please {MessageLink(viewMessageUrl, "view the message", false, recipient)} to reply.";
        }

        string fromMessage = $"{MessageSettings.FromMessageParameter}={recipient.MessageKey}";
        string fromRecipient = $"{MessageSettings.FromRecipientParameter}={recipient.MessageRecipientKey}";

        messageText = messageText
            .Replace($"{MessageSettings.FromMessageParameter}=null", fromMessage)
            .Replace($"{MessageSettings.FromMessageParameter}=0", fromMessage)
            .Replace($"{MessageSettings.FromRecipientParameter}=null", fromRecipient)
            .Replace($"{MessageSettings.FromRecipientParameter}=0", fromRecipient);

        return messageText;
    }

    public string MessageLink(string url, string display, bool addQuotes, IEmailRecipient? recipient = null)
    {
        url += (url.Contains("?") ? "&" : "?")
            + $"{MessageSettings.FromMessageParameter}={recipient?.MessageKey}" +
            $"&{MessageSettings.FromRecipientParameter}={recipient?.MessageRecipientKey}";

        string quote = addQuotes ? "'" : "";
        return $"<a href=\"{url}\">{quote}{display}{quote}</a>";
    }
}
