using Global.Abstractions.Global.Messages;

namespace Data.Abstractions;

public interface IMessageEntity : IArchivableEntity, IMessageBase
{
    int MessageKey { get; set; }
    int SenderKey { get; set; }
}
