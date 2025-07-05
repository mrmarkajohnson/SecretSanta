namespace Global.Abstractions.Areas.Messages;

public interface ISantaMessageBase : IMessageBase
{
    int MessageKey { get; set; }
    public DateTime Sent { get; set; }
    string? GroupName { get; }
    bool ShowAsFromSanta { get; }
}
