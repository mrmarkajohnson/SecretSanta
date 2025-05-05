using Global.Abstractions.Areas.Suggestions;
using Global.Validation;

namespace Data.Entities.Santa;

public class Santa_Suggestion : DeletableBaseEntity, IDeletableEntity, ISuggestionBase,
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

    [MaxLength(SuggestionVal.Suggestion.MaxLength)]
    public string SuggestionText { get; set; } = string.Empty;

    /// <summary>
    /// E.g. things to avoid
    /// </summary>
    public string OtherNotes { get; set; } = string.Empty;

    public virtual ICollection<Santa_SuggestionLink> YearGroupUserLinks { get; set; }
    public virtual ICollection<Santa_Suggestion_Audit> AuditTrail { get; set; }

    public void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes)
    {
        this.AddNewAuditEntry<Santa_Suggestion, Santa_Suggestion_Audit, Santa_Suggestion_AuditChange>(auditTrail, changes);
    }
}
