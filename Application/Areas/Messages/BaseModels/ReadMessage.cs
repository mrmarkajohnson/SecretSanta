using Application.Shared.BaseModels;
using Global.Abstractions.Global.Messages;

namespace Application.Areas.Messages.BaseModels;

internal class ReadMessage : MessageBase, IReadMessage
{
    public int RecipientId { get; set; }
    public IUserNamesBase? Sender { get; set; } = new UserNamesBase();
    public bool Read { get; set; }
    public bool ShowAsFromSanta { get; set; }
    public string SenderName => (ShowAsFromSanta || Sender == null) ? "Santa" : Sender.UserDisplayName;
}
