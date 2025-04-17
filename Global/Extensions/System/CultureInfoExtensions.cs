using System.Globalization;

namespace Global.Extensions.System;

public static class CultureInfoExtensions
{
    public static LocationSelectable CultureLocation(this CultureInfo ci)
    {
        int locationStart = ci.DisplayName.IndexOf("(") + 1;
        int locationEnd = ci.DisplayName.IndexOf(")");
        string location = ci.DisplayName[locationStart..locationEnd];
        var regionInfo = new RegionInfo(ci.Name);

        return new LocationSelectable
        {
            Name = ci.Name,
            Location = location,
            IsoCurrency = regionInfo?.ISOCurrencySymbol,
            CurrencySymbol = regionInfo?.CurrencySymbol,
            CurrencyString = regionInfo != null ? GetCurrencyString(regionInfo) : null
        };
    }

    private static string GetCurrencyString(RegionInfo regionInfo)
    {
        return GetCurrencyString(regionInfo.ISOCurrencySymbol, regionInfo.CurrencySymbol);
    }

    public static string GetCurrencyString(string? isoCurrency, string? currencySymbol)
    {
        isoCurrency = isoCurrency?.Trim() ?? string.Empty;
        currencySymbol = currencySymbol?.Trim() ?? string.Empty;

        return isoCurrency == currencySymbol
            ? (isoCurrency == string.Empty
                ? string.Empty // both empty
                : isoCurrency) // both the same, not empty
            : (currencySymbol == string.Empty
                ? isoCurrency // can't be empty
                : $"{isoCurrency} ({currencySymbol})"); // neither empty but not the same
    }

    public static string GetDefultCurrencyCode(string cultureName)
    {
        var regionInfo = new RegionInfo(cultureName);
        return regionInfo.ISOCurrencySymbol;
    }

    public static string GetDefultCurrencySymbol(string cultureName)
    {
        var regionInfo = new RegionInfo(cultureName);
        return regionInfo.CurrencySymbol;
    }
}

public sealed class LocationSelectable
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? IsoCurrency { get; set; }
    public string? CurrencySymbol { get; set; }
    public string? CurrencyString { get; set; }
}