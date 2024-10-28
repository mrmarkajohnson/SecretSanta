using Global.Settings;

namespace Data.Entities.Shared.Base;

public abstract class AuditBaseEntity : BaseEntity, IAuditBase
{
    public int Id { get; set; }

    public string? UserId { get; set; }
    public virtual Global_User? User { get; set; }

    public GlobalSettings.AuditAction Action { get; set; }
}
