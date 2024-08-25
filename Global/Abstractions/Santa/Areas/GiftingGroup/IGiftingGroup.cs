namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IGiftingGroup
{
    int Id { get; }

    string Name { get; }
    string Description { get; }

    string JoinerToken { get; set; }

    string CultureInfo { get; }
    string? CurrencyCodeOverride { get; }
    string? CurrencySymbolOverride { get; }
}
