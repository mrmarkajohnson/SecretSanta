namespace Data.Entities.Santa;

public class Santa_YearGroupUser : BaseEntity, IAuditableEntity<Santa_YearGroupUser_Audit, Santa_YearGroupUser_AuditChange>
{
    public Santa_YearGroupUser()
    {
        Suggestions = new HashSet<Santa_Suggestion>();
        AuditTrail = new HashSet<Santa_YearGroupUser_Audit>();
    }

    [Key]
    public int Id { get; set; }

    public int YearId { get; set; }
    public virtual required Santa_GiftingGroupYear Year { get; set; }

    public int SantaUserId { get; set; }
    public virtual required Santa_User SantaUser { get; set; }

    public bool? Included { get; set; }

    public int? GivingToUserId { get; set; }
    public virtual Santa_User? GivingToUser { get; set; }

    public virtual ICollection<Santa_Suggestion> Suggestions { get; set; }
    public virtual ICollection<Santa_YearGroupUser_Audit> AuditTrail { get; set; }

    public void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes)
    {
        this.AddNewAuditEntry<Santa_YearGroupUser, Santa_YearGroupUser_Audit, Santa_YearGroupUser_AuditChange>(auditTrail, changes);
    }
}
