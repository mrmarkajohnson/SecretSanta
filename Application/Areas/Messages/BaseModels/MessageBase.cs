using Global.Abstractions.Areas.Messages;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Messages.BaseModels;

public class MessageBase : IMessageBase
{
    [Display(Name = "To")]
    public MessageSettings.MessageRecipientType RecipientType { get; set; }

    [Display(Name = "Title")]
    public required string HeaderText { get; set; }

    [Display(Name = "Message")]
    public required string MessageText { get; set; }

    public bool Important { get; set; }
    public bool CanReply { get; set; }
}
