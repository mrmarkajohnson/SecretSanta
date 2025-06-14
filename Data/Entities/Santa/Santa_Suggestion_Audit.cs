namespace Data.Entities.Santa;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Santa_Suggestion_Audit : AuditBaseEntity, IAuditEntity<Santa_Suggestion, Santa_Suggestion_AuditChange>
{
    public Santa_Suggestion_Audit()
    {
        Changes = new HashSet<Santa_Suggestion_AuditChange>();
    }

    public int ParentKey { get; set; }
    public virtual Santa_Suggestion Parent { get; set; }

    public virtual ICollection<Santa_Suggestion_AuditChange> Changes { get; set; }
}
