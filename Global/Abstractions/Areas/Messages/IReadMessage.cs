using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Global.Messages;

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

    [Display(Name = "From")]
    string SenderName { get; }

    bool Read { get; set; }
}
