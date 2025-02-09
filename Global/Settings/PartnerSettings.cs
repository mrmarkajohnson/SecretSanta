using System.ComponentModel.DataAnnotations;

namespace Global.Settings;

public static class PartnerSettings
{
    public enum RelationshipStatus // Warning: if changing the values, ensure they are matched in the JavaScript
    {
        [Display(Name = "Waiting for your partner to confirm your relationship")]
        ToBeConfirmed = 0,
        [Display(Name = "Please confirm if you are in a relationship")]
        ToConfirm,
        [Display(Name = "We are currently in a relationship")]
        Active,
        [Display(Name = "We were in a relationship; I'd rather not exchange gifts")]
        Ended,
        [Display(Name = "The relationship has already ended")]
        EndedBeforeConfirmation,
        [Display(Name = "We were in a relationship, but I'm happy to exchange gifts")]
        IgnoreOld,
        [Display(Name = "We have never been in a relationship")]
        NotRelationship
    }
}
