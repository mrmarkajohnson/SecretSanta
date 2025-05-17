namespace Global.Abstractions.Areas.Messages;

public interface ISendSantaMessage : IMessageBase
{
    bool ShowAsFromSanta { get; }
}
