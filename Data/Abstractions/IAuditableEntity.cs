namespace Data.Abstractions;

/// <summary>
/// Don't implement this directly, use IAuditableEntity<TAuditEntity, TChangeEntity> instead
/// </summary>
internal interface IAuditableEntity : IEntity
{
    void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes);
}

internal interface IAuditableEntity<TAuditEntity, TChangeEntity> : IAuditableEntity 
    where TAuditEntity : class, IAuditEntity<TChangeEntity>, new()
    where TChangeEntity : class, IAuditChangeEntity, new()
{
    ICollection<TAuditEntity> AuditTrail { get; }
}

internal static class AuditableEntityExtensions
{
    public static void AddNewAuditEntry<TParentEntity, TAuditEntity, TChangeEntity>(this TParentEntity parent,        
        IAuditBase auditTrail, IList<IAuditBaseChange> changes)
            where TParentEntity : class, IAuditableEntity<TAuditEntity, TChangeEntity>
            where TAuditEntity : class, IAuditEntity<TParentEntity, TChangeEntity>, new()
            where TChangeEntity : class, IAuditChangeEntity<TAuditEntity>, new()
    {
        var auditEntity = new TAuditEntity
        {
            Parent = parent,
            Action = auditTrail.Action,
            UserId = auditTrail.UserId,
            Changes = new List<TChangeEntity>()
        };

        var changesList = changes.Select(x => new TChangeEntity // do this separately to link in the auditEntity
        {
            Audit = auditEntity,
            ColumnName = x.ColumnName,
            DisplayName = x.DisplayName,
            OldValue = x.OldValue,
            NewValue = x.NewValue,
        });

        auditEntity.Changes = changesList.ToList();

        parent.AuditTrail.Add(auditEntity);
    }
}
