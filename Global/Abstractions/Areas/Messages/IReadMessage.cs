using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Messages;

public interface IReadMessage : IMessageBase
{
    /// <summary>
    /// Key of the MessageRecipient record
    /// </summary>
    int MessageRecipientKey { get; }

    int MessageKey { get; set; }
    public DateTime Sent { get; set; }

    IUserNamesBase? Sender { get; }
    bool ShowAsFromSanta { get; }

    string SenderName { get; }
    string? GroupName { get; }

    bool Read { get; set; }
}
