namespace Data.Entities.Santa;

public class Santa_SuggestionLink : DeletableBaseEntity,
    IAuditableEntity<Santa_SuggestionLink_Audit, Santa_SuggestionLink_AuditChange>
{
    public Santa_SuggestionLink()
    {
        AuditTrail = new HashSet<Santa_SuggestionLink_Audit>();
    }

    [Key]
    public int SuggestionLinkKey { get; set; }

    public int SuggestionKey { get; set; }
    public virtual required Santa_Suggestion Suggestion { get; set; }

    /// <summary>
    /// Identifies the group member who makes the suggestion
    /// </summary>
    public int YearGroupUserKey { get; set; }

    /// <summary>
    /// Identifies the group member who makes the suggestion
    /// </summary>
    public virtual required Santa_YearGroupUser YearGroupUser { get; set; }

    public virtual ICollection<Santa_SuggestionLink_Audit> AuditTrail { get; set; }

    public void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes)
    {
        this.AddNewAuditEntry<Santa_SuggestionLink, Santa_SuggestionLink_Audit, Santa_SuggestionLink_AuditChange>(auditTrail, changes);
    }
}
