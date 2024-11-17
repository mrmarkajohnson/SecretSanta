using Global.Abstractions.Global.Shared;

namespace Application.Santa.Areas.GiftingGroup.BaseModels;

public abstract class UserNamesBase : IUserAllNames
{
    private string? _userDisplayName;

    public string Forename { get; set; } = "";
    public string? MiddleNames { get; set; }
    public string Surname { get; set; } = "";

    public string UserDisplayName
    {
        get => _userDisplayName ?? Forename + " " + Surname;
        set => _userDisplayName = value;
    }
}
