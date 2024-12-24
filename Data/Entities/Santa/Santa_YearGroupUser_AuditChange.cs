namespace Data.Entities.Santa;

public class Santa_YearGroupUser_AuditChange : AuditBaseChange, IAuditChangeEntity<Santa_YearGroupUser_Audit>
{
    public virtual Santa_YearGroupUser_Audit Audit { get; set; } = new();
}
