using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Validation;

namespace Data.Entities.Santa;

public class Santa_GiftingGroup : DeletableBaseEntity, IDeletableEntity, IGiftingGroup
{
    public Santa_GiftingGroup()
    {
        UserLinks = new HashSet<Santa_GiftingGroupUser>();
        Years = new HashSet<Santa_GiftingGroupYear>();
        MemberApplications = new HashSet<Santa_GiftingGroupApplication>();
    }

    [Key]
    public int Id { get; set; }

    [Required, MaxLength(GiftingGroupVal.Name.MaxLength)]
    public string Name { get; set; } = "";

    [Required, MaxLength(GiftingGroupVal.Description.MaxLength)]
    public string Description { get; set; } = "";

    [Required, MaxLength(GiftingGroupVal.JoinerToken.MaxLength)]
    public string JoinerToken { get; set; } = "";

    [Required, MaxLength(GiftingGroupVal.CultureInfo.MaxLength)]
    public string CultureInfo { get; set; } = "en-GB";

    [MaxLength(GiftingGroupVal.CurrencyCodeOverride.MaxLength)]
    public string? CurrencyCodeOverride { get; set; } = "GBP";

    [MaxLength(GiftingGroupVal.CurrencySymbolOverride.MaxLength)]
    public string? CurrencySymbolOverride { get; set; } = "£";

    public virtual ICollection<Santa_GiftingGroupUser> UserLinks { get; set; }
    public virtual ICollection<Santa_GiftingGroupYear> Years { get; set; }
    public virtual ICollection<Santa_GiftingGroupApplication> MemberApplications { get; set; }
}
