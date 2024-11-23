namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IGiftingGroupYear : IGiftingGroupYearBase
{
    string GiftingGroupName { get; }
    string CurrencyCode { get; set; }
    string CurrencySymbol { get; set; }

    bool Calculated { get; set; }
    bool RecalculationRequired { get; set; }
    bool Calculate { get; set; }

    IList<IYearGroupUserBase> GroupMembers { get; }
}
