using Global.Abstractions.Shared;
using Global.Settings;

namespace Global.Abstractions.Areas.Messages;

public interface IEmailRecipient : IUserNamesBase
{
    int MessageKey { get; }
    int MessageRecipientKey { get; }

    bool EmailConfirmed { get; }
    MessageSettings.EmailPreference ReceiveEmails { get; }
    bool DetailedEmails { get; }
}

public static class EmailRecipientExtensions
{
    public static bool CanReceiveEmails(this IEmailRecipient recipient)
    {
        return recipient.EmailConfirmed 
            && recipient.Email.IsNotEmpty() 
            && recipient.ReceiveEmails != MessageSettings.EmailPreference.None;
    }
}
