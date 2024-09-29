namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IGiftingGroup : IGiftingGroupBase
{
    int Id { get; }

    string CultureInfo { get; }
    string? CurrencyCodeOverride { get; }
    string? CurrencySymbolOverride { get; }
}