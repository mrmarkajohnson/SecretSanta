using System.Globalization;

namespace Global.Settings;

public static class GlobalSettings
{
    public static readonly IList<CultureInfo> AvailableCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        .Where(x => x.Name.StartsWith("en"))
        .Where(x => !x.Name.StartsWith("en-0") && !x.Name.StartsWith("en-1"))
        .ToList();
}
