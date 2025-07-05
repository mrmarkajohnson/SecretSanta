using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Messages;

public interface IReadMessage : ISantaMessageBase
{
    /// <summary>
    /// Key of the MessageRecipient record
    /// </summary>
    int MessageRecipientKey { get; }

    IUserNamesBase? Sender { get; }

    string SenderName { get; }

    bool Read { get; set; }
    bool SentMessage { get; set; }
}
