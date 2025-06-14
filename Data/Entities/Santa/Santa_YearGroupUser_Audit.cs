namespace Data.Entities.Santa;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Santa_YearGroupUser_Audit : AuditBaseEntity, IAuditEntity<Santa_YearGroupUser, Santa_YearGroupUser_AuditChange>
{
    public Santa_YearGroupUser_Audit()
    {
        Changes = new HashSet<Santa_YearGroupUser_AuditChange>();
    }

    public int ParentKey { get; set; }
    public virtual Santa_YearGroupUser Parent { get; set; }

    public virtual ICollection<Santa_YearGroupUser_AuditChange> Changes { get; set; }
}