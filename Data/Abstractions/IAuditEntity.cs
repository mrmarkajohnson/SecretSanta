namespace Data.Abstractions;

/// <summary>
/// Don't implement this directly, use IAuditEntity<TParentEntity, TChangeEntity> instead
/// </summary>
internal interface IAuditEntity : IAuditBase, IEntity
{    
    int ParentId { get; set; }
}

/// <summary>
/// Don't implement this directly, use IAuditEntity<TParentEntity, TChangeEntity> instead
/// </summary>
internal interface IAuditEntity<TChangeEntity> : IAuditEntity
    where TChangeEntity : class, IAuditChangeEntity, new()
{
    ICollection<TChangeEntity> Changes { get; set; }
}

internal interface IAuditEntity<TParentEntity, TChangeEntity> : IAuditEntity<TChangeEntity>
    where TParentEntity : IEntity 
    where TChangeEntity : class, IAuditChangeEntity, new()
{
    TParentEntity Parent { get; set; }
}
