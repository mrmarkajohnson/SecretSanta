using Application.Shared.BaseModels;
using Global.Abstractions.Global.Messages;

namespace Application.Areas.Messages.BaseModels;

public class ReadMessage : MessageBase, IReadMessage
{
    public int MessageRecipientKey { get; set; }
    public int MessageKey { get; set; }
    public DateTime Sent { get; set; }

    public IUserNamesBase? Sender { get; set; } = new UserNamesBase();
    public bool ShowAsFromSanta { get; set; }
    public string SenderName => (ShowAsFromSanta || Sender == null) ? "Santa" : Sender.UserDisplayName;

    public bool Read { get; set; }
}
