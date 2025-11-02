using Global.Settings;

namespace Global.Abstractions.Shared;

public interface IUserEmailDetails : IEmailPreferences, IHaveAnEmail, IHaveEmailConfirmed
{
}

public static class EmaiDetailsExtensions
{
    public static bool CanReceiveEmails(this IUserEmailDetails recipient)
    {
        return recipient.EmailConfirmed
            && recipient.Email.IsNotEmpty()
            && recipient.ReceiveEmails != MessageSettings.EmailPreference.None;
    }
}
