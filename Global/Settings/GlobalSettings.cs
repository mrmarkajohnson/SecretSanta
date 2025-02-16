using System.Globalization;

namespace Global.Settings;

public static class GlobalSettings
{
    public static readonly IList<CultureInfo> AvailableCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        .Where(x => x.Name.StartsWith("en"))
        .Where(x => !x.Name.StartsWith("en-0") && !x.Name.StartsWith("en-1"))
        .ToList();

    public enum AuditAction
    {
        Create = 0,
        Update = 1,
        Delete = 2,
        Archive = 3,
        View = 4
    }

    public const string StandardGroupWidth = "col-lg-6 col-md-8 col-sm-10 col-12";
    public const string StandardFormGroup = $"form-group-ib {StandardGroupWidth} mb-3";
    public const string StandardFullWidthGridContainer = "ps-4 pe-4 col-12 col-lg-11";
}
