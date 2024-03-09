namespace Data.Entities.Base;

public abstract class DeletableBaseEntity : ArchivableBaseEntity, IDeletableEntity
{
    public DateTime? DateDeleted { get; set; }
}
