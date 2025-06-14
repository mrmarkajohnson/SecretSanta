namespace Application.Areas.Home.ViewModels;

public sealed class MainMenuVm
{
    public HomeVm HomeModel { get; set; } = new();
    public bool Small { get; set; }
    public string DropdownClass => Small ? "dropstart" : string.Empty;

    public List<string> LoggedInMenuItems =
    [
        "YearMenu",
        "MessagesMenu",
        "GiftingMenu",
        "PartnersMenu",
        "AccountMenu",
    ];
}