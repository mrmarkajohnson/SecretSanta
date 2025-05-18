using Global.Names;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GlobalSettings;
using static Global.Settings.IdentitySettings;

namespace Application.Shared.Identity;

public class GlobalUser : CoreIdentityUser, IGlobalUser
{
    /// <summary>
    /// Allows middle names or other details to be added when needed, e.g. if two people in a list have the same name
    /// </summary>
    private string? _userDisplayName;

    [Display(Name = UserDisplayNames.Forename), StringLength(UserVal.Forename.MaxLength, MinimumLength = UserVal.Forename.MinLength)]
    public string Forename { get; set; } = string.Empty;

    [Display(Name = "Middle Names"), MaxLength(UserVal.MiddleNames.MaxLength)]
    public string? MiddleNames { get; set; }

    [Display(Name = UserDisplayNames.PreferredNameType)]
    public PreferredNameOption PreferredNameType { get; set; }

    [Display(Name = UserDisplayNames.PreferredFirstName), MaxLength(UserVal.PreferredFirstName.MaxLength)]
    public string? PreferredFirstName { get; set; }

    [Display(Name = "Surname"), StringLength(UserVal.Surname.MaxLength, MinimumLength = UserVal.Surname.MinLength)]
    public string Surname { get; set; } = string.Empty;

    [Display(Name = "Preferred Gender")]
    public Gender Gender { get; set; }

    public bool SecurityQuestionsSet { get; set; }

    public string UserDisplayName
    {
        get => _userDisplayName ?? this.DisplayName(false);
        set => _userDisplayName = value;
    }
}
