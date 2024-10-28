using static Global.Settings.GlobalSettings;

namespace Data.Abstractions;

public interface IAuditBase
{
    int Id { get; set; }
    string? UserId { get; set; }
    AuditAction Action { get; set; }    
}
