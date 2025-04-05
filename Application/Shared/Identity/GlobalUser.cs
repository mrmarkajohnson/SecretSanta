using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class GlobalUser : CoreIdentityUser, IGlobalUser
{
    /// <summary>
    /// Allows middle names or other details to be added when needed, e.g. if two people in a list have the same name
    /// </summary>
    private string? _userDisplayName;

    [Display(Name = "First Name"), StringLength(UserVal.Forename.MaxLength, MinimumLength = UserVal.Forename.MinLength)]
    public string Forename { get; set; } = string.Empty;

    [Display(Name = "Middle Names"), MaxLength(UserVal.MiddleNames.MaxLength)]
    public string? MiddleNames { get; set; }

    [Display(Name = "Surname"), StringLength(UserVal.Surname.MaxLength, MinimumLength = UserVal.Surname.MinLength)]
    public string Surname { get; set; } = string.Empty;

    public bool SecurityQuestionsSet { get; set; }

    public string UserDisplayName
    {
        get => _userDisplayName ?? Forename + " " + Surname;
        set => _userDisplayName = value;
    }
}
