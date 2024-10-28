namespace Data.Abstractions;

/// <summary>
/// Don't implement this directly, use IAuditChangeEntity<TAuditEntity> instead
/// </summary>
public interface IAuditChangeEntity : IAuditBaseChange
{
    int AuditId { get; set; }
}

internal interface IAuditChangeEntity<TAuditEntity> : IAuditChangeEntity where TAuditEntity : IAuditEntity
{
    public TAuditEntity Audit { get; set; }
}

