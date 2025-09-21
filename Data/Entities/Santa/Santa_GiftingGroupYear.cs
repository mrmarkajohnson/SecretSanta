using Global.Abstractions.Areas.GiftingGroup;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Santa;

public class Santa_GiftingGroupYear : DeletableBaseEntity, IGiftingGroupYearBase,
    IAuditableEntity<Santa_GiftingGroupYear_Audit, Santa_GiftingGroupYear_AuditChange>
{
    public Santa_GiftingGroupYear()
    {
        Users = new HashSet<Santa_YearGroupUser>();
        AuditTrail = new HashSet<Santa_GiftingGroupYear_Audit>();
        Messages = new HashSet<Santa_Message>();
    }

    [Key]
    public int GiftingGroupYearKey { get; set; }

    [Required, Length(4, 4)]
    public int CalendarYear { get; set; }

    [Precision(10, 2)]
    public decimal? Limit { get; set; }

    public required string CurrencyCode { get; set; }
    public required string CurrencySymbol { get; set; }

    public int GiftingGroupKey { get; set; }
    public virtual required Santa_GiftingGroup GiftingGroup { get; set; }

    public virtual ICollection<Santa_YearGroupUser> Users { get; set; }
    public virtual ICollection<Santa_GiftingGroupYear_Audit> AuditTrail { get; set; }
    public virtual ICollection<Santa_Message> Messages { get; set; }

    public IEnumerable<Santa_GiftingGroupUser> ValidGroupMembers()
    {
        DateTime firstDayOfNextYear = new DateTime(CalendarYear + 1, 1, 1);

        return GiftingGroup.Members
            .Where(x => x.DateDeleted == null && (x.DateArchived == null || x.DateArchived < firstDayOfNextYear));
    }

    public List<Santa_YearGroupUser> ParticipatingMembers() => Users
        .Where(x => x.Included == true)
        .Where(x => ValidGroupMembers().Any(y => y.SantaUserKey == x.SantaUserKey))
        .ToList();

    public void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes)
    {
        this.AddNewAuditEntry<Santa_GiftingGroupYear, Santa_GiftingGroupYear_Audit, Santa_GiftingGroupYear_AuditChange>(auditTrail, changes);
    }

    public bool Calculated()
    {
        return ParticipatingMembers().Any(x => x.RecipientSantaUserKey != null);
    }
}