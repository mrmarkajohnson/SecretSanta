namespace Data.Abstractions;

public interface IArchivableEntity : IEntity
{    
    public DateTime? DateArchived { get; }
}
