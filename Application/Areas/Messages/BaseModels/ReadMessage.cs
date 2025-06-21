using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Messages;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Messages.BaseModels;

public class ReadMessage : MessageBase, IReadMessage
{
    public int MessageRecipientKey { get; set; }
    public int MessageKey { get; set; }
    public DateTime Sent { get; set; }

    public IUserNamesBase? Sender { get; set; } = new UserNamesBase(); // must be initialised for the mapping
    public bool ShowAsFromSanta { get; set; }

    [Display(Name = "From")]
    public string SenderName => (ShowAsFromSanta || Sender == null) ? "Santa" : Sender.UserDisplayName;

    [Display(Name = "For Group")]
    public string? GroupName { get; set; }

    public bool Read { get; set; }
}
