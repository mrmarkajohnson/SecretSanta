namespace Data.Abstractions;

public interface IAuditBaseChange
{
    string ColumnName { get; set; }
    string DisplayName { get; set; }
    string OldValue { get; set; }
    string NewValue { get; set; }
}
