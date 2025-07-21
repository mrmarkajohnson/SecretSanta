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

    public const string CenterFlexWrapContainer = "d-flex flex-wrap text-center justify-content-center align-content-center";
    public const string CenterFlexNoWrapContainer = "d-flex text-center justify-content-center align-items-center";
    public const string EvenSpacingFlexGroup = "d-flex flex-wrap justify-content-between text-center";
    public const string StandardPageWidth = "col-md-10 col-sm-11 col-12";
    public const string StandardGroupWidth = "col-xl-7 col-lg-8 col-md-10 col-sm-11 col-12";    
    public const string ModalGroupWidth = "col-xl-9 col-lg-10 col-md-12 col-12";
    public const string StandardFormGroup = $"form-group-ib {StandardGroupWidth} mb-3";
    public const string StandardFullWidthGridContainer = "ps-4 pe-4 col-12 col-lg-11 mb-2";
    public const string StandardCardWidth = "col-12 col-md-6 col-lg-4";
}
