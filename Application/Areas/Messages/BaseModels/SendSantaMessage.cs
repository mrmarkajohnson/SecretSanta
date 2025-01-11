using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

internal class SendSantaMessage : MessageBase, ISendSantaMessage
{
    public required bool ShowAsFromSanta { get; set; }
}
