using Application.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class SetupGiftingGroupYearQuery : GiftingGroupBaseQuery<IGiftingGroupYear>
{
    private readonly int _giftingGroupKey;
    private readonly int _calendarYear;

    public SetupGiftingGroupYearQuery(int giftingGroupKey, int? calendarYear = null)
    {
        _giftingGroupKey = giftingGroupKey;
        _calendarYear = calendarYear ?? GlobalSettings.CurrentYear;
    }

    protected async override Task<IGiftingGroupYear> Handle()
    {
        if (_giftingGroupKey <= 0)
        {
            throw new NotFoundException("Gifting Group");
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await GetGiftingGroupUserLink(_giftingGroupKey, true);
        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

        Santa_GiftingGroupYear? dbGiftingGroupYear = dbGiftingGroup.Years
            .Where(GroupYearExpressions.IsActive(false))
            .FirstOrDefault(x => x.CalendarYear == _calendarYear);

        GiftingGroupYear giftingGroupYear = new();
        DateTime firstDayOfNextYear = DateHelper.FirstDayOfNextYear(_calendarYear);
        var validGroupMembers = dbGiftingGroup.ActiveMembers(firstDayOfNextYear);

        if (dbGiftingGroupYear != null)
        {
            Mapper.Map(dbGiftingGroupYear, giftingGroupYear);

            var missingGroupMembers = validGroupMembers
                .Where(x => giftingGroupYear.GroupMembers.Any(y => y.SantaUserKey == x.SantaUserKey) == false)
                .Select(x => Mapper.Map(x, new YearGroupUser()))
                .ToList();

            if (missingGroupMembers.Any())
            {
                giftingGroupYear.GroupMembers.AddRange(missingGroupMembers);
            }
        }
        else
        {
            Mapper.Map(dbGiftingGroup, giftingGroupYear);

            giftingGroupYear.CalendarYear = _calendarYear;

            giftingGroupYear.GroupMembers = validGroupMembers
                .Select(x => Mapper.Map(x, new YearGroupUser()))
                .ToList();
        }

        if (string.IsNullOrEmpty(giftingGroupYear.CurrencyCode))
        {
            giftingGroupYear.CurrencyCode = CultureInfoExtensions.GetDefultCurrencyCode(dbGiftingGroup.CultureInfo);
        }

        if (string.IsNullOrEmpty(giftingGroupYear.CurrencySymbol))
        {
            giftingGroupYear.CurrencySymbol = CultureInfoExtensions.GetDefultCurrencySymbol(dbGiftingGroup.CultureInfo);
        }

        giftingGroupYear.GroupMembers = giftingGroupYear.GroupMembers
            .Select(x => x.UnHash())
            .OrderBy(x => x.DisplayFirstName()).ThenBy(x => x.Surname)
            .ToList();

        return giftingGroupYear;
    }
}
