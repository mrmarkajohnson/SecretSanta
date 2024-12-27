using Application.Shared.Identity;
using Global.Abstractions.Global.Shared;

namespace Application.Santa.Areas.GiftingGroup.BaseModels;

public class UserNamesBase : BaseUser, IUserNamesBase
{
    /// <summary>
    /// Allows middle names or other details to be added when needed, e.g. if two people in a list have the same name
    /// </summary>
    private string? _userDisplayName;

    public string Forename { get; set; } = string.Empty;
    public string? MiddleNames { get; set; }
    public string Surname { get; set; } = string.Empty;

    public string UserDisplayName
    {
        get => _userDisplayName ?? Forename + " " + Surname;
        set => _userDisplayName = value;
    }
}
