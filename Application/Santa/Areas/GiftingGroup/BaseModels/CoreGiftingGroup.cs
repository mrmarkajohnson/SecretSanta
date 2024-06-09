using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace Application.Santa.Areas.GiftingGroup.BaseModels;

public class CoreGiftingGroup : IGiftingGroup
{
    public CoreGiftingGroup()
    {
        //UserLinks = new List<GiftingGroupUser>();
        //Years = new List<GiftingGroupYear>();
    }

    public int Id { get; set; }

    [Required, StringLength(GiftingGroupVal.Name.MaxLength, MinimumLength = GiftingGroupVal.Name.MinLength)]
    public string Name { get; set; } = "";

    [Required, StringLength(GiftingGroupVal.Description.MaxLength, MinimumLength = GiftingGroupVal.Description.MinLength)]
    public string Description { get; set; } = "";

    [Required, StringLength(GiftingGroupVal.JoinerToken.MaxLength, MinimumLength = GiftingGroupVal.JoinerToken.MinLength)]
    public string JoinerToken { get; set; } = "";

    [Required, StringLength(GiftingGroupVal.CultureInfo.MaxLength, MinimumLength = GiftingGroupVal.CultureInfo.MinLength)]
    public string CultureInfo { get; set; } = "en-GB";

    [MaxLength(GiftingGroupVal.CurrencyCodeOverride.MaxLength)]
    public string? CurrencyCodeOverride { get; set; } = "GBP";

    [MaxLength(GiftingGroupVal.CurrencySymbolOverride.MaxLength)]
    public string? CurrencySymbolOverride { get; set; } = "£";

    //public IList<GiftingGroupUser> UserLinks { get; set; }
    //public IList<GiftingGroupYear> Years { get; set; }
}
