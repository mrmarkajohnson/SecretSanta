using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Names;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace Application.Santa.Areas.GiftingGroup.BaseModels;

public class CoreGiftingGroup : IGiftingGroup
{
    public CoreGiftingGroup()
    {
    }

    public int Id { get; set; }

    [Required, StringLength(GiftingGroupVal.Name.MaxLength, MinimumLength = GiftingGroupVal.Name.MinLength)]
    [Display(Name = "Group Name")]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(GiftingGroupVal.Description.MaxLength, MinimumLength = GiftingGroupVal.Description.MinLength)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Joiner Token"), Required, StringLength(GiftingGroupVal.JoinerToken.MaxLength, MinimumLength = GiftingGroupVal.JoinerToken.MinLength)]
    public string JoinerToken { get; set; } = string.Empty;

    [Display(Name = GiftingGroupNames.CultureInfo), Required, StringLength(GiftingGroupVal.CultureInfo.MaxLength, MinimumLength = GiftingGroupVal.CultureInfo.MinLength)]
    public string CultureInfo { get; set; } = "en-GB";

    [MaxLength(GiftingGroupVal.CurrencyCodeOverride.MaxLength)]
    public string? CurrencyCodeOverride { get; set; } = "GBP";

    [MaxLength(GiftingGroupVal.CurrencySymbolOverride.MaxLength)]
    public string? CurrencySymbolOverride { get; set; } = "£";

    [Display(Name = "First Year")]
    public int FirstYear { get; set; }
}
