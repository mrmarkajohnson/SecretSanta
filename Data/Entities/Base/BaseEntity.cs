namespace Data.Entities.Base;

public abstract class BaseEntity : IEntity
{
    public BaseEntity()
    {
        DateCreated = DateTime.Now;
    }

    public DateTime DateCreated { get; }
}
