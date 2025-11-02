using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroupYearBase : IHaveACalendarYear
{
    int GiftingGroupKey { get; set; }

    decimal? Limit { get; set; }
    string CurrencyCode { get; set; }
    string CurrencySymbol { get; set; }
}