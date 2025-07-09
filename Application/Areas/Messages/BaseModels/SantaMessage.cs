using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Messages;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.BaseModels;

public class SantaMessage : SantaMessageBase, ISantaMessage
{
    public int MessageRecipientKey { get; set; }

    public IUserNamesBase? Sender { get; set; } = new UserNamesBase(); // must be initialised for the mapping    

    [Display(Name = "From")]
    public string SenderName => IsSentMessage 
        ? (ShowAsFromSanta ? "You (as Santa)" : "You") 
        : (ShowAsFromSanta || Sender == null) ? "Santa" : Sender.UserDisplayName;

    public bool IsSentMessage { get; set; }
    public bool Read { get; set; }
}
