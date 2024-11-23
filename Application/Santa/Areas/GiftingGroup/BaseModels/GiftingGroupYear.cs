using Global.Abstractions.Santa.Areas.GiftingGroup;
using System.ComponentModel.DataAnnotations;

namespace Application.Santa.Areas.GiftingGroup.BaseModels;

public class GiftingGroupYear : IGiftingGroupYear
{
    public int GiftingGroupId { get; set; }
    public string GiftingGroupName { get; set; } = string.Empty;

    [Required, Length(4, 4)]
    public int Year { get; set; }

    [Display(Name = "Spending Limit")]
    public decimal? Limit { get; set; }

    public string CurrencyCode { get; set; } = "£";
    public string CurrencySymbol { get; set; } = "GBP";

    public bool Calculated { get; set; }
    public bool RecalculationRequired { get; set; }
    public bool Calculate { get; set; }

    public List<YearGroupUserBase> GroupMembers { get; set; } = new();

    IList<IYearGroupUserBase> IGiftingGroupYear.GroupMembers => GroupMembers.ToList<IYearGroupUserBase>();
}
