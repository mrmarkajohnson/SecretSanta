namespace Data.Entities.Shared;

public class Global_User_Audit : AuditBaseEntity, IAuditEntity<Global_User, Global_User_AuditChange>
{
    public Global_User_Audit()
    {
        Changes = new HashSet<Global_User_AuditChange>();
    }

    public string ParentId { get; set; } = string.Empty;
    public virtual Global_User Parent { get; set; }

    public virtual ICollection<Global_User_AuditChange> Changes { get; set; }
}
