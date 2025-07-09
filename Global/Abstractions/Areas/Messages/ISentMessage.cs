namespace Global.Abstractions.Areas.Messages;

public interface ISentMessage : ISantaMessageBase
{
    string? ReplyToName { get; }
}
