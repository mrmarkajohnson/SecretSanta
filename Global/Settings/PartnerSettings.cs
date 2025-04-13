using System.ComponentModel.DataAnnotations;

namespace Global.Settings;

public static class PartnerSettings
{
    public enum RelationshipStatus // Warning: if changing the values, ensure they are matched in the JavaScript
    {
        [Display(Name = "Waiting for your partner to confirm")]
        ToBeConfirmed = 0,
        [Display(Name = "Please confirm if you are in a relationship")]
        ToConfirm,
        [Display(Name = "We're currently in a relationship")]
        Active,
        [Display(Name = "We broke up, and I don't want to exchange gifts")]
        Ended,
        [Display(Name = "The relationship has already ended")]
        EndedBeforeConfirmation,
        [Display(Name = "We broke up, but I'm happy to exchange gifts")]
        IgnoreOld,
        [Display(Name = "We've never been together, and I'm happy to exchange gifts")]
        IgnoreNonRelationship,
        [Display(Name = "We've never been together, but I don't want to exchange gifts")]
        Avoid
    }
}
