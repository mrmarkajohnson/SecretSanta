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
            CurrencyString = regionInfo != null 
                ? (regionInfo.ISOCurrencySymbol != regionInfo.CurrencySymbol 
                    ? $"{regionInfo.ISOCurrencySymbol} ({regionInfo.CurrencySymbol})"
                    : regionInfo.ISOCurrencySymbol)
                : null
        };
    }
}

public class LocationSelectable
{
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    public string? IsoCurrency { get; set; }
    public string? CurrencySymbol { get; set; }
    public string? CurrencyString { get; set; }
}