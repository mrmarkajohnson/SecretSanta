using Application.Shared.Identity;
using Global.Names;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GlobalSettings;

namespace Application.Shared.BaseModels;

public class UserNamesBase : BaseUser, IUserNamesBase
{
    /// <summary>
    /// Allows middle names or other details to be added when needed, e.g. if two people in a list have the same name
    /// </summary>
    private string? _userDisplayName;

    [Display(Name = UserDisplayNames.Forename)]
    public string Forename { get; set; } = string.Empty;

    [Display(Name = "Middle Names")]
    public string? MiddleNames { get; set; }

    [Display(Name = "Preferred Name")]
    public string? PreferredFirstName { get; set; }

    public bool PreferredIsNickname { get; set; }

    [Display(Name = "Surname")]
    public string Surname { get; set; } = string.Empty;

    [Display(Name = "Preferred Gender")]
    public Gender Gender { get; set; }

    public string UserDisplayName
    {
        get => _userDisplayName ?? this.DisplayName(false);
        set => _userDisplayName = value;
    }
}
