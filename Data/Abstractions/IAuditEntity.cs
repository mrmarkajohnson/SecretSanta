using Data.Entities.Shared;

namespace Data.Abstractions;

/// <summary>
/// Don't implement this directly, use IAuditEntity<TParentEntity, TChangeEntity> instead
/// Required for ApplicationDbContext
/// </summary>
internal interface IAuditEntity : IAuditBase, IEntity
{
    Global_User? User { get; set; }
}

/// <summary>
/// Don't implement this directly, use IAuditEntity<TParentEntity, TChangeEntity> instead
/// Required for IAuditableEntity<TAuditEntity, TChangeEntity>
/// </summary>
internal interface IAuditEntity<TChangeEntity> : IAuditEntity
    where TChangeEntity : class, IAuditChangeEntity, new()
{
    ICollection<TChangeEntity> Changes { get; set; }
}

internal interface IAuditEntity<TParentEntity, TChangeEntity> : IAuditEntity<TChangeEntity>
    where TParentEntity : class, IEntity
    where TChangeEntity : class, IAuditChangeEntity, new()
{
    TParentEntity Parent { get; set; }
}
