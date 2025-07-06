using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Areas.Messages;

public interface IMessageBase : IHasMessageRecipientType
{
    [Display(Name = "Title")]
    string HeaderText { get; }

    [Display(Name = "Message")]
    string MessageText { get; set; }

    bool Important { get; set; }
    bool CanReply { get; set; }
}
