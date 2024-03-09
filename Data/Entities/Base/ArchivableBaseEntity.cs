
namespace Data.Entities.Base;

public abstract class ArchivableBaseEntity : BaseEntity, IArchivableEntity
{
    public DateTime? DateArchived { get; set; }
}
