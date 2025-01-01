using System.ComponentModel.DataAnnotations;

namespace Global.Settings;

public static class PartnerSettings
{
    public enum RelationshipStatus // Warning: if changing the values, ensure they are matched in the JavaScript
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
        IgnoreOld,
        [Display(Name = "Not a relationship")]
        NotRelationship
    }
}
