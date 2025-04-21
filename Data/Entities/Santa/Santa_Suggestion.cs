using Global.Abstractions.Areas.Suggestions;

namespace Data.Entities.Santa;

public class Santa_Suggestion : ArchivableBaseEntity, IArchivableEntity, ISuggestionBase,
    IAuditableEntity<Santa_Suggestion_Audit, Santa_Suggestion_AuditChange>
{
    public Santa_Suggestion()
    {
        YearGroupUserLinks = new HashSet<Santa_SuggestionLink>();
        AuditTrail = new HashSet<Santa_Suggestion_Audit>();
    }

    [Key]
    public int SuggestionKey { get; set; }

    public int SantaUserKey { get; set; }
    public virtual required Santa_User SantaUser { get; set; }

    public int Priority { get; set; }

    public required string SuggestionText { get; set; }

    /// <summary>
    /// E.g. things to avoid
    /// </summary>
    public required string OtherNotes { get; set; }

    public virtual ICollection<Santa_SuggestionLink> YearGroupUserLinks { get; set; }
    public virtual ICollection<Santa_Suggestion_Audit> AuditTrail { get; set; }

    public void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes)
    {
        this.AddNewAuditEntry<Santa_Suggestion, Santa_Suggestion_Audit, Santa_Suggestion_AuditChange>(auditTrail, changes);
    }
}
