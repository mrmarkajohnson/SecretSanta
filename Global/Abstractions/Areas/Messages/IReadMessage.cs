using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Messages;

public interface IReadMessage : ISantaMessage, IHasMessageChain, IHaveAGroupKey
{
}