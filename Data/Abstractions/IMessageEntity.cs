using Global.Abstractions.Global.Messages;

namespace Data.Abstractions;

public interface IMessageEntity : IArchivableEntity, IMessageBase
{
    int Id { get; set; }
    int SenderId { get; set; }
}
