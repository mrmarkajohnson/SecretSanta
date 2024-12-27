using System.ComponentModel.DataAnnotations;

namespace Global.Settings;

public static class PartnerSettings
{
    public enum RelationshipStatus
    {
        [Display(Name = "Awaiting confirmation by partner")]
        ToBeConfirmed = 0,
        [Display(Name = "Waiting for you to confirm")]
        ToConfirm,
        [Display(Name = "Confirmed")]
        Active,
        [Display(Name = "Ended")]
        Ended,
        [Display(Name = "Old and can be ignored")]
        IgnoreOld
    }
}
