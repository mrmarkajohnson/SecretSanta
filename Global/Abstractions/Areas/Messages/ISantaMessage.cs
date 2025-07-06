using Global.Abstractions.Shared;
using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Areas.Messages;

public interface ISantaMessage : ISantaMessageBase
{
    /// <summary>
    /// Key of the MessageRecipient record
    /// </summary>
    int MessageRecipientKey { get; }

    IUserNamesBase? Sender { get; }

    [Display(Name = "From")]
    string SenderName { get; }

    bool IsSentMessage { get; set; }
    bool Read { get; set; }
}
