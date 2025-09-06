using Global.Settings;

namespace Global.Abstractions.Shared;

public interface IEmailPreferences
{    
    MessageSettings.EmailPreference ReceiveEmails { get; }
    bool DetailedEmails { get; }
}
