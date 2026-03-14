using Data.Attributes;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Settings;
using Global.Validation;

namespace Data.Entities.Santa;

public class Santa_GiftingGroup : ArchivableBaseEntity, IGiftingGroup,
    IAuditableEntity<Santa_GiftingGroup_Audit, Santa_GiftingGroup_AuditChange>
{
    public Santa_GiftingGroup()
    {
        Members = new HashSet<Santa_GiftingGroupUser>();
        Years = new HashSet<Santa_GiftingGroupYear>();
        MemberApplications = new HashSet<Santa_GiftingGroupApplication>();
        AuditTrail = new HashSet<Santa_GiftingGroup_Audit>();
        Invitations = new HashSet<Santa_Invitation>();
    }

    [Key]
    public int GiftingGroupKey { get; set; }

    [Required, MaxLength(GiftingGroupVal.Name.MaxLength)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(GiftingGroupVal.Description.MaxLength)]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(GiftingGroupVal.JoinerToken.MaxLength)]
    public string JoinerToken { get; set; } = string.Empty;

    [Audit(GiftingGroupNames.CultureInfo), Required, MaxLength(GiftingGroupVal.CultureInfo.MaxLength)]
    public string CultureInfo { get; set; } = "en-GB";

    [Audit("Currency Code"), MaxLength(GiftingGroupVal.CurrencyCodeOverride.MaxLength)]
    public string? CurrencyCodeOverride { get; set; } = "GBP";

    [Audit("Currency Symbol"), MaxLength(GiftingGroupVal.CurrencySymbolOverride.MaxLength)]
    public string? CurrencySymbolOverride { get; set; } = "£";

    public int FirstYear { get; set; }

    public virtual ICollection<Santa_GiftingGroupUser> Members { get; set; }
    public virtual ICollection<Santa_GiftingGroupYear> Years { get; set; }
    public virtual ICollection<Santa_GiftingGroupApplication> MemberApplications { get; set; }
    public virtual ICollection<Santa_GiftingGroup_Audit> AuditTrail { get; set; }
    public virtual ICollection<Santa_Invitation> Invitations { get; internal set; }

    public LocationSelectable? GetLocation() => GlobalSettings.Locations.FirstOrDefault(x => x.Name == CultureInfo);
    public string GetCurrencyCode() => CurrencyCodeOverride ?? GetLocation()?.CurrencyString ?? "GBP";
    public string GetCurrencySymbol() => CurrencySymbolOverride ?? GetLocation()?.CurrencySymbol ?? "£";

    public void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes)
    {
        this.AddNewAuditEntry<Santa_GiftingGroup, Santa_GiftingGroup_Audit, Santa_GiftingGroup_AuditChange>(auditTrail, changes);
    }

    public Santa_User? Recipient(int santaUserKey, int calendarYear)
    {
        return Years.Where(x => x.CalendarYear == calendarYear)
            .SelectMany(y => y.Users.Where(u => u.SantaUserKey == santaUserKey && u.RecipientSantaUserKey > 0))
            .FirstOrDefault()?
            .SantaUser;
    }

    public IEnumerable<Santa_User> GroupAdministrators()
    {
        return ActiveMembers().Where(x => x.GroupAdmin).Select(x => x.SantaUser);
    }

    public IEnumerable<Santa_GiftingGroupUser> OtherMembers(Santa_User? dbExcludingUser = null)
    {
        return ActiveMembers().Where(x => dbExcludingUser == null || x.SantaUserKey != dbExcludingUser.SantaUserKey);
    }

    public IEnumerable<Santa_GiftingGroupUser> ActiveMembers(DateTime? archivedAfter = null)
    {
        return Members.Where(x => x.DateArchived == null || (archivedAfter != null && x.DateArchived > archivedAfter))
            .Where(x => x.SantaUser.DateArchived == null);
    }

    public int CurrentYear()
    {
        return Math.Max(DateCreated.Year, GlobalSettings.CurrentYear);
    }
}
