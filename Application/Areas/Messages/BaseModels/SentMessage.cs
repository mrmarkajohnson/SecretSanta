using Application.Shared.BaseModels;
using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

public class SentMessage : SantaMessageBase, ISentMessage
{
    public UserNamesBase? ReplyTo { get; set; }
    public string? ReplyToName => ReplyTo?.DisplayName() ?? "Santa";
}
