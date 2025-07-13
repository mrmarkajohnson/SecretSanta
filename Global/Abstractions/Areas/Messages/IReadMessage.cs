namespace Global.Abstractions.Areas.Messages;

public interface IReadMessage : ISantaMessage
{
    int GiftingGroupKey { get; }
    IList<ISantaMessage> PreviousMessages { get; set; }
}