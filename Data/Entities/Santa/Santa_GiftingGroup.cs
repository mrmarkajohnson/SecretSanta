using Data.Attributes;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Names;
using Global.Validation;

namespace Data.Entities.Santa;

public class Santa_GiftingGroup : DeletableBaseEntity, IDeletableEntity, IGiftingGroup, 
    IAuditableEntity<Santa_GiftingGroup_Audit, Santa_GiftingGroup_AuditChange>
{
    public Santa_GiftingGroup()
    {
        UserLinks = new HashSet<Santa_GiftingGroupUser>();
        Years = new HashSet<Santa_GiftingGroupYear>();
        MemberApplications = new HashSet<Santa_GiftingGroupApplication>();
        AuditTrail = new HashSet<Santa_GiftingGroup_Audit>();
    }

    [Key]
    public int Id { get; set; }

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

    public virtual ICollection<Santa_GiftingGroupUser> UserLinks { get; set; }
    public virtual ICollection<Santa_GiftingGroupYear> Years { get; set; }
    public virtual ICollection<Santa_GiftingGroupApplication> MemberApplications { get; set; }
    public virtual ICollection<Santa_GiftingGroup_Audit> AuditTrail { get; set; }

    public void AddAuditEntry(IAuditBase auditTrail, IList<IAuditBaseChange> changes)
    {
        this.AddNewAuditEntry<Santa_GiftingGroup, Santa_GiftingGroup_Audit, Santa_GiftingGroup_AuditChange>(auditTrail, changes);
    }
}
