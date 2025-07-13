namespace Global.Abstractions.Areas.Messages;

public interface IReadMessage : ISantaMessage, IHasMessageChain
{
    int GiftingGroupKey { get; }    
}