using System.ComponentModel.DataAnnotations;

namespace Global.Settings;

public static class PartnerSettings
{
    public enum RelationshipStatus // Warning: if changing the values, ensure they are matched in the JavaScript
    {
        [Display(Name = "Waiting for your partner to confirm")]
        ToBeConfirmed = 0,
        [Display(Name = "Please confirm this relationship")]
        ToConfirm,
        [Display(Name = "We're currently in a relationship")]
        Active,
        [Display(Name = "We split, I don't want to exchange gifts")]
        Ended,
        [Display(Name = "The relationship already ended")]
        EndedBeforeConfirmation,
        [Display(Name = "We split, I'm happy to exchange gifts")]
        IgnoreOld,
        [Display(Name = "We were never together, I'm happy to exchange gifts")]
        IgnoreNonRelationship,
        [Display(Name = "We were never together, I don't want to exchange gifts")]
        Avoid
    }
}
