using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

internal class EmailRecipient : UserNamesBase, IEmailRecipient
{
    public int MessageKey { get; set; }
    public int MessageRecipientKey { get; set; }

    public bool EmailConfirmed { get; set; }
    public MessageSettings.EmailPreference ReceiveEmails { get; set; }
    public bool DetailedEmails { get; set; }

    public string Greeting { get; set; } = string.Empty;
    public bool SkipPreferencesFooter { get; set; }
}