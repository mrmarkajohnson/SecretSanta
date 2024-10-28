namespace Data.Entities.Shared.Base;

public abstract class AuditBaseChange : IAuditBaseChange
{
    public int Id { get; set; }
    public int AuditId { get; set; }

    public string ColumnName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
}
