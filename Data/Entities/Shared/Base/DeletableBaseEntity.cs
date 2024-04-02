namespace Data.Entities.Shared.Base;

public abstract class DeletableBaseEntity : ArchivableBaseEntity, IDeletableEntity
{
    public DateTime? DateDeleted { get; set; }
}
