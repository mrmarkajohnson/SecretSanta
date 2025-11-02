using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroup : IGiftingGroupBase, IHaveAGroupKey
{
    string CultureInfo { get; }
    string? CurrencyCodeOverride { get; }
    string? CurrencySymbolOverride { get; }

    int FirstYear { get; }
}