using Global.Abstractions.Areas.GiftingGroup;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.GiftingGroup.BaseModels;

public class GiftingGroupYear : GiftingGroupYearBase, IGiftingGroupYear
{
    public bool Calculated { get; set; }
    public bool RecalculationRequired { get; set; }
    public string? PreviousYearsWarning { get; set; }

    [Display(Name = "Assign givers and receivers")]
    public YearCalculationOption CalculationOption { get; set; }

    public List<YearGroupUserBase> GroupMembers { get; set; } = new();

    IList<IYearGroupUserBase> IGiftingGroupYear.GroupMembers => GroupMembers.ToList<IYearGroupUserBase>();
}
