using Global.Abstractions.Areas.Messages;

namespace Data.Abstractions;

public interface IMessageEntity : IArchivableEntity, IMessageBase
{
    int MessageKey { get; set; }
    int SenderKey { get; set; }
}
