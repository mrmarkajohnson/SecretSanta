namespace Data.Entities.Shared;

public class Global_User_AuditChange : AuditBaseChange, IAuditChangeEntity<Global_User_Audit>
{
    public virtual Global_User_Audit Audit { get; set; } = new();
}
