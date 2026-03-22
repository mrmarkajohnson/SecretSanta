using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Messages;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.MessageSettings;

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
    public string SenderName => FromDescription();

    public bool Read { get; set; }

    private string FromDescription()
    {
        if (IsSentMessage)
        {
            return FromDescriptionForSentMessage();
        }
        else
        {
            if (ShowAsFromSanta || Sender == null)
            {
                return RecipientType is MessageRecipientType.GiftRecipient ? $"Your gift giver for '{GroupName}'" : "Santa";
            }

            return RecipientType is MessageRecipientType.Gifter ? $"{Sender.UserDisplayName} (as gift recipient)" : Sender.UserDisplayName;
        }
    }
}
