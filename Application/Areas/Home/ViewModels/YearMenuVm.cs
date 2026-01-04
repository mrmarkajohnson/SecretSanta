using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.Home.ViewModels;

public class YearMenuVm : IHaveACalendarYear
{
    public int CalendarYear { get; set; }
    public IList<IUserGiftingGroup> GiftingGroups { get; set; } = [];
}
