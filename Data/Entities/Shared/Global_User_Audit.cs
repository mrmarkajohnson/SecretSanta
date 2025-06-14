namespace Data.Entities.Shared;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

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
