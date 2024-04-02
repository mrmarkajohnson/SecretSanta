namespace Data.Entities.Shared.Base;

public abstract class ArchivableBaseEntity : BaseEntity, IArchivableEntity
{
    public DateTime? DateArchived { get; set; }
}
