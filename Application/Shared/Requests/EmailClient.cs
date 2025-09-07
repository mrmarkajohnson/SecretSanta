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
            result.AddError("No recipients were found.");
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
            result.AddWarning("No valid recipients were found.");
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
            if (recipient.Email == null)
                return; // for the compiler, should be caught earlier

            var mimeMessage = new MimeMessage();

            string toAddress = (_hostEnvironment.IsProduction() || message.HeaderText == StandardPhrases.TestEmailHeader)
                ? recipient.Email
                : _mailSettings.TestAddress;

            mimeMessage.From.Add(new MailboxAddress(_mailSettings.FromName, _mailSettings.FromAddress));
            mimeMessage.To.Add(new MailboxAddress(recipient.DisplayName(), toAddress));
            mimeMessage.Subject = message.HeaderText;
            mimeMessage.Body = new TextPart("html") { Text = messageText };

            client.Send(mimeMessage);
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

        if (message.HeaderText == StandardPhrases.ConfirmationEmailHeader)
            return $"{messageText}:<br/><br/><i>{message.MessageText}</i>";

        if (!recipient.DetailedEmails)
        {
            messageText += ".";
            AddViewDetails(ref messageText, recipient, viewMessageUrl);
            AddEmailPreferencesFooter(ref messageText);

            return messageText;
        }

        messageText += $":<br/><br/><i>{message.MessageText}</i><br/><br/>";
        AddViewAndReplyDetails(ref messageText, message, recipient, viewMessageUrl);
        ReplaceEmptyFromMessageKeys(ref messageText, recipient);
        AddEmailPreferencesFooter(ref messageText);

        return messageText;
    }

    private void AddViewDetails(ref string messageText, IEmailRecipient recipient, string? viewMessageUrl)
    {
        if (viewMessageUrl.IsNotEmpty()) // just in case!
        {
            messageText += $"<br/><br/>Please {MessageLink(viewMessageUrl, "click here", false, recipient)} to view the message.";
        }
    }

    private void AddViewAndReplyDetails(ref string messageText, ISantaMessage message, IEmailRecipient recipient, string? viewMessageUrl)
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
    }

    private void AddEmailPreferencesFooter(ref string messageText)
    {
        if (MessageSettings.EmailPreferencesUrl.IsNotEmpty())
        {
            messageText += $"<br/><br/><small>You are receiving this message as a user of the Secret Santa system. " +
                $"To change your e-mail preferences, {MessageLink(MessageSettings.EmailPreferencesUrl, "click here", false)}.</small>";
        }
    }

    /// <summary>
    /// Ensure any addition to the URL (added before saving) to mark the message as read has the message and receipient keys
    /// </summary>
    private static void ReplaceEmptyFromMessageKeys(ref string messageText, IEmailRecipient recipient)
    {
        string fromMessage = $"{MessageSettings.FromMessageParameter}={recipient.MessageKey}";
        string fromRecipient = $"{MessageSettings.FromRecipientParameter}={recipient.MessageRecipientKey}";

        messageText = messageText
            .Replace($"{MessageSettings.FromMessageParameter}=null", fromMessage)
            .Replace($"{MessageSettings.FromMessageParameter}=0", fromMessage)
            .Replace($"{MessageSettings.FromRecipientParameter}=null", fromRecipient)
            .Replace($"{MessageSettings.FromRecipientParameter}=0", fromRecipient);
    }

    public string MessageLink(string url, string display, bool addQuotes, IEmailRecipient? recipient = null)
    {
        if (recipient?.MessageKey > 0)
        {
            url += UrlHelper.ParameterDelimiter(url) +
                $"{MessageSettings.FromMessageParameter}={recipient?.MessageKey}" +
                $"&{MessageSettings.FromRecipientParameter}={recipient?.MessageRecipientKey}"; // add message IDs to mark the message as read
        }

        string quote = addQuotes ? "'" : "";
        return $"<a href=\"{url}\">{quote}{display}{quote}</a>";
    }
}
