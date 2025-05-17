using Application.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class SetupGiftingGroupYearQuery : GiftingGroupBaseQuery<IGiftingGroupYear>
{
    private readonly int _giftingGroupKey;
    private readonly int _year;

    public SetupGiftingGroupYearQuery(int giftingGroupKey, int? year = null)
    {
        _giftingGroupKey = giftingGroupKey;
        _year = year ?? DateTime.Today.Year;
    }

    protected async override Task<IGiftingGroupYear> Handle()
    {
        if (_giftingGroupKey == 0)
        {
            throw new NotFoundException("Gifting Group");
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await GetGiftingGroupUserLink(_giftingGroupKey, true);
        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;
        Santa_GiftingGroupYear? dbGiftingGroupYear = dbGiftingGroup.Years.FirstOrDefault(x => x.CalendarYear == _year);

        GiftingGroupYear giftingGroupYear = new();
        DateTime firstDayOfNextYear = new DateTime(_year + 1, 1, 1);

        var validGroupMembers = dbGiftingGroup.UserLinks
                .Where(x => x.DateDeleted == null && (x.DateArchived == null || x.DateArchived < firstDayOfNextYear));

        if (dbGiftingGroupYear != null)
        {
            Mapper.Map(dbGiftingGroupYear, giftingGroupYear);

            var missingGroupMembers = validGroupMembers
                .Where(x => giftingGroupYear.GroupMembers.Any(y => y.SantaUserKey == x.SantaUserKey) == false)
                .Select(x => Mapper.Map(x, new YearGroupUserBase()))
                .ToList();

            if (missingGroupMembers.Any())
            {
                //missingGroupMembers.ForEach(x => x.Included = true);
                giftingGroupYear.GroupMembers.AddRange(missingGroupMembers);
            }
        }
        else
        {
            Mapper.Map(dbGiftingGroup, giftingGroupYear);

            giftingGroupYear.CalendarYear = _year;

            giftingGroupYear.GroupMembers = validGroupMembers
                .Select(x => Mapper.Map(x, new YearGroupUserBase()))
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
