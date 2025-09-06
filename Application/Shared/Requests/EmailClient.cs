using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using Global.Abstractions.Areas.Messages;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MimeKit;

namespace Application.Shared.Requests;

internal class EmailClient : IEmailClient
{
    public EmailClient(IServiceProvider services)
    {
        _mailSettings = services.GetRequiredService<IMailSettings>();
        _mapper = services.GetRequiredService<IMapper>();
        _hostEnvironment = services.GetRequiredService<IHostEnvironment>();
    }

    private readonly IMailSettings _mailSettings;
    private readonly IMapper _mapper;
    private readonly IHostEnvironment _hostEnvironment;

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
        var message = _mapper.Map<ISantaMessage>(dbMessage);

        return SendMessage(message, recipients);
    }

    public ValidationResult SendMessage(ISantaMessage message, List<IEmailRecipient> recipients)
    {
        var result = new ValidationResult();

        List<IEmailRecipient> validRecipients = recipients
            .Where(x => x.CanReceiveEmails())
            .Where(x => message.Important || x.ReceiveEmails != MessageSettings.EmailPreference.ImportantOnly)
            .ToList();

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

    private void SendMessage(ISantaMessage message, IEmailRecipient recipient, SmtpClient client, ValidationResult result)
    {
        if (recipient.IdentificationHashed)
            recipient.UnHash();

        bool canSend = CheckValidRecipient(recipient, result);
        if (!canSend)
        {
            return;
        }

        string messageText = GetMessageText(message, recipient);

        try
        {
            var mimeMessage = new MimeMessage();

            string toAddress = _hostEnvironment.IsProduction() || message.HeaderText == MessageSettings.TestEmailHeader
                ? recipient.Email
                : _mailSettings.TestAddress;

            mimeMessage.From.Add(new MailboxAddress(_mailSettings.FromName, _mailSettings.FromAddress));
            mimeMessage.To.Add(new MailboxAddress(recipient.DisplayName(), toAddress));
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

    private static bool CheckValidRecipient(IEmailRecipient recipient, ValidationResult result)
    {
        if (!recipient.EmailConfirmed)
        {
            result.AddWarning($"{recipient.FullName()} has not confirmed {recipient.Gender.Posessive()} e-mail address.", nameof(recipient.Email));
            return false;
        }

        if (recipient.ReceiveEmails == MessageSettings.EmailPreference.None)
        {
            result.AddWarning($"{recipient.FullName()} has chosen not to receive e-mails.", nameof(recipient.Email));
            return false;
        }

        if (recipient.ReceiveEmails == MessageSettings.EmailPreference.None)
        {
            result.AddWarning($"{recipient.FullName()} has chosen not to receive e-mails.", nameof(recipient.Email));
            return false;
        }

        if (string.IsNullOrEmpty(recipient.Email))
        {
            result.AddWarning($"{recipient.FullName()}'s e-mail address is empty.", nameof(recipient.Email));
            return false;
        }

        if (!EmailHelper.IsEmail(recipient.Email))
        {
            result.AddWarning($"{recipient.FullName()}'s e-mail address is not valid.", nameof(recipient.Email));
            return false;
        }

        return true;
    }

    private string GetMessageText(ISantaMessage message, IEmailRecipient recipient)
    {
        string? viewMessageUrl = MessageSettings.ViewMessageUrl.IsNotEmpty() && recipient.MessageKey > 0
            ? $"?messageKey={recipient.MessageKey}&messageRecipientKey={recipient.MessageRecipientKey}"
            : null;

        string messageFrom = message.ShowAsFromSanta || message.Sender == null
            ? "Santa"
            : $"{message.Sender.DisplayName(false)} for Secret Santa";

        string messageText = $"Dear {recipient.DisplayFirstName()},<br/><br/>" +
            $"You have a message from {messageFrom}";

        if (!recipient.DetailedEmails)
        {
            messageText = ".";

            if (viewMessageUrl.IsNotEmpty()) // just in case!
            {
                messageText += $".<br/><br/> Please {MessageLink(viewMessageUrl, "click here", false, recipient)} to view the message.";
            }

            return messageText;
        }

        messageText += ":<br/><br/><i>" + message.MessageText + $"</i><br/><br/>";
        messageText = AddViewAndReplyDetails(message, recipient, viewMessageUrl, messageText);
        messageText = ReplaceEmptyKeys(recipient, messageText);

        return messageText;
    }

    private string AddViewAndReplyDetails(ISantaMessage message, IEmailRecipient recipient, string? viewMessageUrl, string messageText)
    {
        if (message.CanReply)
        {
            messageText += $"You cannot reply directly to this e-mail.";

            if (viewMessageUrl.IsNotEmpty()) // just in case!
            {
                messageText += $" Please {MessageLink(viewMessageUrl, "view the message", false, recipient)} to reply.";
            }
        }
        /* // TODO: Restore this once users can report issues and abuse
        else if (viewMessageUrl.IsNotEmpty()) // just in case!
        {
            messageText += $"You cannot reply, but you can " +
                $"{MessageLink(viewMessageUrl, "view the message", false, recipient)}" +
                $"to report an issue or abuse.";            
        }
        */
        return messageText;
    }

    private static string ReplaceEmptyKeys(IEmailRecipient recipient, string messageText)
    {
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
        if (recipient?.MessageKey > 0)
        {
            url += (url.Contains("?") ? "&" : "?") +
                $"{MessageSettings.FromMessageParameter}={recipient?.MessageKey}" +
                $"&{MessageSettings.FromRecipientParameter}={recipient?.MessageRecipientKey}";
        }

        string quote = addQuotes ? "'" : "";
        return $"<a href=\"{url}\">{quote}{display}{quote}</a>";
    }
}
