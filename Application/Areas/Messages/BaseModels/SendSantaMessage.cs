using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

public class SendSantaMessage : MessageBase, ISendSantaMessage
{
    public required bool ShowAsFromSanta { get; set; }
}
