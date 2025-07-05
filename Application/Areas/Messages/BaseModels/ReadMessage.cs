using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Messages;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Messages.BaseModels;

public class ReadMessage : SantaMessageBase, IReadMessage
{
    public int MessageRecipientKey { get; set; }

    public IUserNamesBase? Sender { get; set; } = new UserNamesBase(); // must be initialised for the mapping    

    [Display(Name = "From")]
    public string SenderName => SentMessage ? (ShowAsFromSanta ? "You (as Santa)" : "You") : (ShowAsFromSanta || Sender == null) ? "Santa" : Sender.UserDisplayName;

    public bool Read { get; set; }
    public bool SentMessage { get; set; }
}
