using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Messages;

public interface ISantaMessage : ISantaMessageBase
{
    /// <summary>
    /// Key of the MessageRecipient record
    /// </summary>
    int MessageRecipientKey { get; }

    IUserNamesBase? Sender { get; }
    string SenderName { get; }

    bool IsSentMessage { get; set; }
    bool Read { get; set; }
}
