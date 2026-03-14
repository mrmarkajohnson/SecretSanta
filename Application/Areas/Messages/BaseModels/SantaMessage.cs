using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Messages;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Messages.BaseModels;

public class SantaMessage : SantaMessageBase, ISantaMessage
{
    private IUserNamesBase? _sender = new UserNamesBase(); // must be initialised for the mapping
    
    public int MessageRecipientKey { get; set; }

    public IUserNamesBase? Sender
    {
        get
        {
            if (!IsSentMessage && !ShowAsFromSanta && _sender != null && _sender.IdentificationHashed)
            {
                _sender.UnHash();
            }

            return _sender;
        }
        set => _sender = value; 
    }

    [Display(Name = "From")]
    public string SenderName => IsSentMessage 
        ? (ShowAsFromSanta ? "You (as Santa)" : "You") 
        : (ShowAsFromSanta || Sender == null) ? "Santa" : Sender.UserDisplayName;

    public bool Read { get; set; }
}
