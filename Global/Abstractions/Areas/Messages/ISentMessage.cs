using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Messages;

public interface ISentMessage : ISantaMessageShared
{
    IUserNamesBase? SentTo { get; }
}