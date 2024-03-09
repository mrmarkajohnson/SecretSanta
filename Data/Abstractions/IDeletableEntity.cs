namespace Data.Abstractions;

public interface IDeletableEntity : IArchivableEntity
{    
    DateTime? DateDeleted { get; set; }
}
