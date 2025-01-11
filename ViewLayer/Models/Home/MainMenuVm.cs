namespace ViewLayer.Models.Home;

public class MainMenuVm
{
    public HomeVm HomeModel { get; set; } = new();
    public bool Small { get; set; }
    public string DropdownClass => Small ? "dropstart" : string.Empty;

    public List<string> LoggedInMenuItems =
    [
        "MessagesMenu",
        "GiftingMenu",
        "PartnersMenu",
        "AccountMenu"
    ];
}
