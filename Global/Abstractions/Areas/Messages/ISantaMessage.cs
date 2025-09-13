using Global.Abstractions.Shared;
using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Areas.Messages;

public interface ISantaMessage : ISantaMessageShared
{
    /// <summary>
    /// Key of the MessageRecipient record
    /// </summary>
    int MessageRecipientKey { get; }

    IUserNamesBase? Sender { get; }

    [Display(Name = "From")]
    string SenderName { get; }

    bool Read { get; set; }
    bool IsTestMessage => HeaderText == StandardPhrases.TestEmailHeader;
}
