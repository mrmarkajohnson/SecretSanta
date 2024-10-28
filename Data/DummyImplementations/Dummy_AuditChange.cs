namespace Data.DummyImplementations;

internal class Dummy_AuditChange : IAuditBaseChange
{
    public required string ColumnName { get; set; }
    public required string DisplayName { get; set; }
    public required string OldValue { get; set; }
    public required string NewValue { get; set; }
}
