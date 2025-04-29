using static Global.Settings.GlobalSettings;

namespace Data.Abstractions;

public interface IAuditBase
{
    int AuditKey { get; set; }
    string? GlobalUserId { get; set; }
    AuditAction Action { get; set; }
}
