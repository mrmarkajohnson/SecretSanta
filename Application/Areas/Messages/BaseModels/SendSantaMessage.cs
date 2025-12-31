using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

public sealed class SendSantaMessage : MessageBase, ISendSantaMessage
{
    public required bool ShowAsFromSanta { get; set; }
}
