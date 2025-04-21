namespace Data.Entities.Santa;

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
