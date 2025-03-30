namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroupYearBase
{
    int GiftingGroupKey { get; set; }
    int Year { get; set; }

    decimal? Limit { get; set; }
    string CurrencyCode { get; set; }
    string CurrencySymbol { get; set; }
}
