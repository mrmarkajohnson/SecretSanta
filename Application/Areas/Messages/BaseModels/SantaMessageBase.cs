using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Messages;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.BaseModels;

public abstract class SantaMessageBase : MessageBase, ISantaMessageShared
{
    private SantaMessage? _replyTo; // for mapping into only; usee notes on ReplyTo below    
    private UserNamesBase? _specificRecipient; // for mapping into only; see notes on SpecificRecipient below
    private bool _useSpecificRecipient; // for mapping into only; use UseSpecificRecipient otherwise
    private bool _showAsToSanta; // for mapping into only

    public int MessageKey { get; set; }
    public DateTime Sent { get; set; }
    public bool ShowAsFromSanta { get; set; }

    [Display(Name = "For Group")]
    public string? GroupName { get; set; }

    public virtual bool IsSentMessage { get; set; }

    public SantaMessage? ReplyTo
    {
        // as we have to map from something, if it isn't a reply, we map from the mssage itself
        // the getter then returns null if that happesn
        get => _replyTo?.MessageKey == MessageKey ? null : _replyTo;
        set => _replyTo = value;
    }

    public virtual string? ReplyToName
    {
        get => ShowAsToSanta ? "Santa" : ReplyTo?.SenderName;
        set { } // allows override for the ViewModel via mapping
    }

    internal bool UseSpecificRecipient 
    {
        get => _useSpecificRecipient && IsSentMessage && !ShowAsToSanta && SpecificRecipientTypes.Contains(RecipientType);
        set => _useSpecificRecipient = value;
    }

    public UserNamesBase? SpecificRecipient
    {
        // as we have to map from something, if it isn't for a specific recipient, we map from sender
        // the getter then returns null if that happesn
        get => UseSpecificRecipient && !ShowAsToSanta ? _specificRecipient : null;
        set => _specificRecipient = value;
    }

    public virtual string? SpecificRecipientName
    {
        get => ShowAsToSanta
        ? "Santa"
        : SpecificRecipient?.UserDisplayName;
        set { } // allows override for the ViewModel via mapping
    }

    public bool ShowAsToSanta
    {
        get
        {
            if (IsSentMessage && RecipientType == MessageRecipientType.Gifter)
                _showAsToSanta = true;

            return _showAsToSanta;
        }
        set
        {
            if (value == true)
                _showAsToSanta = true;
        }
    }

    protected string FromDescriptionForSentMessage()
    {
        return RecipientType switch
        {
            MessageRecipientType.Gifter => "You (as gift recipient)",
            MessageRecipientType.GiftRecipient => "You (as gift giver)",
            _ => ShowAsFromSanta ? "You (as Santa)" : "You"
        };
    }
}
