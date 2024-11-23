using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class GlobalUser : CoreIdentityUser, IGlobalUser
{
    [Display(Name = "First Name"), StringLength(UserVal.Forename.MaxLength, MinimumLength = UserVal.Forename.MinLength)]
    public string Forename { get; set; } = string.Empty;

    [Display(Name = "Middle Names"), MaxLength(UserVal.MiddleNames.MaxLength)]
    public string? MiddleNames { get; set; }

    [Display(Name = "Surname"), StringLength(UserVal.Surname.MaxLength, MinimumLength = UserVal.Surname.MinLength)]
    public string Surname { get; set; } = string.Empty;

    public bool SecurityQuestionsSet { get; set; }
}
