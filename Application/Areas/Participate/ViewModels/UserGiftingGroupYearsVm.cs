using Global.Abstractions.Areas.Participate;

namespace Application.Areas.Participate.ViewModels;

public sealed class UserGiftingGroupYearsVm : IHaveACalendarYear
{
    public UserGiftingGroupYearsVm(int calendarYear, IQueryable<IUserGiftingGroupYear> userGiftingGroupYears)
    {
        CalendarYear = calendarYear;
        UserGiftingGroupYears = userGiftingGroupYears;
    }

    public int CalendarYear { get; set; }
    public IQueryable<IUserGiftingGroupYear> UserGiftingGroupYears { get; set; }
}
