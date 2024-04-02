using Data.Entities.Shared.Base;

namespace Data.Entities.Santa;

public class Santa_GiftingGroup : DeletableBaseEntity, IDeletableEntity
{
    public Santa_GiftingGroup()
    {
        UserLinks = new HashSet<Santa_GiftingGroupUser>();
        Years = new HashSet<Santa_GiftingGroupYear>();
    }

    [Key]
    public int Id { get; set; }

    [Required, Length(4, 150)]
    public string Name { get; set; } = "";

    [Required, Length(6, 250)]
    public string Description { get; set; } = "";

    [Required, Length(8, 15)]
    public string JoinerToken { get; set; } = "";

    [Required, Length(3, 8)]
    public string CultureInfo { get; set; } = "en-GB";

    [MaxLength(4)]
    public string? CurrencyCodeOverride { get; set; } = "GBP";

    [MaxLength(3)]
    public string? CurrencySymbolOverride { get; set; } = "£";

    public virtual ICollection<Santa_GiftingGroupUser> UserLinks { get; set; }
    public virtual ICollection<Santa_GiftingGroupYear> Years { get; set; }
}
