namespace Data.Entities.Santa;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Santa_SuggestionLink_Audit : AuditBaseEntity, IAuditEntity<Santa_SuggestionLink, Santa_SuggestionLink_AuditChange>
{
    public Santa_SuggestionLink_Audit()
    {
        Changes = new HashSet<Santa_SuggestionLink_AuditChange>();
    }

    public int ParentKey { get; set; }
    public virtual Santa_SuggestionLink Parent { get; set; }

    public virtual ICollection<Santa_SuggestionLink_AuditChange> Changes { get; set; }
}
