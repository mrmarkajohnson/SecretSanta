using System.ComponentModel.DataAnnotations;
using static Global.Settings.MessageSettings;

namespace Global.Abstractions.Areas.Messages;

public interface IMessageBase
{
    [Display(Name = "To")]
    MessageRecipientType RecipientTypes { get; }

    [Display(Name = "Subject")]
    string HeaderText { get; }

    [Display(Name = "Message")]
    string MessageText { get; set; }

    bool Important { get; set; }
    bool CanReply { get; set; }
}
