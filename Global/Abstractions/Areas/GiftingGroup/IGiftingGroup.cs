namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroup : IGiftingGroupBase
{
    int GiftingGroupKey { get; }

    string CultureInfo { get; }
    string? CurrencyCodeOverride { get; }
    string? CurrencySymbolOverride { get; }

    int FirstYear { get; }
}