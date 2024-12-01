using System.ComponentModel.DataAnnotations;

namespace Global.Settings;

public static class GiftingGroupSettings
{
    public enum YearCalculationOption
    {
        [Display(Name = "Keep existing")]
        None = 0,

        [Display(Name = "Assign now")]
        Calculate = 1,

        [Display(Name = "Cancel and reset")]
        Cancel = 2
    }

    public const int MinimumParticipatingMembers = 3;
}
