using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Global.Settings;

public static class GlobalSettings
{
    public static readonly IList<CultureInfo> AvailableCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
        .Where(x => x.Name.StartsWith("en"))
        .Where(x => !x.Name.StartsWith("en-0") && !x.Name.StartsWith("en-1"))
        .ToList();

    public enum Gender
    {
        Other = 0,
        Female = 1,
        Male = 2
    }

    public enum AuditAction
    {
        Create = 0,
        Update = 1,
        Delete = 2,
        Archive = 3,
        View = 4
    }

    public enum YesNoNotSure
    {
        No = 1,
        Yes = 2,
        [Display(Name = "Not Sure Yet")]
        NotSure = 3
    }

    public const string StandardGroupWidth = "col-lg-6 col-md-8 col-sm-10 col-12";
    public const string StandardFormGroup = $"form-group-ib {StandardGroupWidth} mb-3";
    public const string StandardFullWidthGridContainer = "ps-4 pe-4 col-12 col-lg-11";
    public const string StandardCardWidth = "col-12 col-md-6 col-lg-4";
}
