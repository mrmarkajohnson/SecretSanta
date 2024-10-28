namespace Data.Abstractions;

public interface IAuditChangeEntity : IAuditBaseChange
{
    int AuditId { get; set; }
}

internal interface IAuditChangeEntity<TAuditEntity> : IAuditChangeEntity where TAuditEntity : IAuditEntity
{
    public TAuditEntity Audit { get; set; }
}

