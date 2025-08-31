using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using Global.Abstractions.Areas.Messages;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace Application.Shared.Requests;

internal class EmailClient
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
            .ProjectTo<IUserNamesBase>(_mapper.ConfigurationProvider)
            .ToList();

        recipients.ForEach(x => x.UnHash());
        return SendMessage(dbMessage, recipients);
    }

    public ValidationResult SendMessage(IMessageBase message, List<IUserNamesBase> recipients)
    {
        var result = new ValidationResult();

        List<IUserNamesBase> validRecipients = recipients.Where(x => x.Email.IsNotEmpty()).ToList();

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

                foreach (IUserNamesBase recipient in validRecipients)
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

    private void SendMessage(IMessageBase message, IUserNamesBase recipient, SmtpClient client, ValidationResult result)
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

        try
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(_mailSettings.FromName, _mailSettings.FromAddress));
            mimeMessage.To.Add(new MailboxAddress(recipient.DisplayName(), recipient.Email));
            mimeMessage.Subject = message.HeaderText;
            mimeMessage.Body = new TextPart("html") { Text = message.MessageText };

            client.Send(mimeMessage);
            return;
        }
        catch (Exception ex)
        {
            result.AddError($"Could not send e-mail to {recipient.FullName()}. {ex.Message}", nameof(recipient.Email));
        }
    }


}
