using Global.Abstractions.Areas.GiftingGroup;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.GiftingGroup.BaseModels;

public class GiftingGroupYearBase : IGiftingGroupYearBase
{
    public int GiftingGroupKey { get; set; }
    public string GiftingGroupName { get; set; } = string.Empty;

    [Display(Name = "Spending Limit")]
    public decimal? Limit { get; set; }

    public string CurrencyCode { get; set; } = "GBP";
    public string CurrencySymbol { get; set; } = "£";

    public int Year { get; set; }
}
