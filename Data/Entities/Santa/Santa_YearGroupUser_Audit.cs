namespace Data.Entities.Santa;

public class Santa_YearGroupUser_Audit : AuditBaseEntity, IAuditEntity<Santa_YearGroupUser, Santa_YearGroupUser_AuditChange>
{
    public Santa_YearGroupUser_Audit()
    {
        Changes = new HashSet<Santa_YearGroupUser_AuditChange>();
    }

    public int ParentId { get; set; }
    public virtual Santa_YearGroupUser Parent { get; set; }

    public virtual ICollection<Santa_YearGroupUser_AuditChange> Changes { get; set; }
}