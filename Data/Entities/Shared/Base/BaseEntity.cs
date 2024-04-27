namespace Data.Entities.Shared.Base;

public abstract class BaseEntity : IEntity
{
    public BaseEntity()
    {
        DateCreated = DateTime.Now;
    }

    public DateTime DateCreated { get; set; }
}
