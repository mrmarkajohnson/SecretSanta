using Global.Settings;

namespace Data.Entities.Shared.Base;

public abstract class AuditBaseEntity : BaseEntity, IAuditBase
{
    [Key]
    public int AuditKey { get; set; }

    public string? GlobalUserId { get; set; }
    public virtual Global_User? GlobalUser { get; set; }

    public GlobalSettings.AuditAction Action { get; set; }
}
