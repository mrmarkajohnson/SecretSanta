using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class GlobalUser : CoreIdentityUser, IGlobalUser
{
    [Display(Name = "First Name"), StringLength(UserDetails.Forename.MaxLength, MinimumLength = UserDetails.Forename.MinLength)]
    public required string Forename { get; set; }

    [Display(Name = "Middle Names"), MaxLength(UserDetails.MiddleNames.MaxLength)]
    public string? MiddleNames { get; set; }

    [Display(Name = "Surname"), StringLength(UserDetails.Surname.MaxLength, MinimumLength = UserDetails.Surname.MinLength)]
    public required string Surname { get; set; }

    public bool SecurityQuestionsSet { get; set; }
}
