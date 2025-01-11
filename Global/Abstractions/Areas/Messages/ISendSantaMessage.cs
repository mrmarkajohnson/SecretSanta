using Global.Abstractions.Global.Messages;

namespace Global.Abstractions.Areas.Messages;

public interface ISendSantaMessage : IMessageBase
{
    bool ShowAsFromSanta { get; }
}
