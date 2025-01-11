using Application.Areas.Account.Actions;
using Application.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Global.Extensions.System;

namespace Application.Areas.GiftingGroup.Queries;

public class SetupGiftingGroupYearQuery : GiftingGroupBaseQuery<IGiftingGroupYear>
{
    private readonly int _groupId;
    private readonly int _year;

    public SetupGiftingGroupYearQuery(int groupId, int? year = null)
    {
        _groupId = groupId;
        _year = year ?? DateTime.Today.Year;
    }

    protected async override Task<IGiftingGroupYear> Handle()
    {
        if (_groupId == 0)
        {
            throw new NotFoundException("Gifting Group");
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await GetGiftingGroupUserLink(_groupId, true);
        Santa_GiftingGroup dbGroup = dbGiftingGroupLink.GiftingGroup;
        Santa_GiftingGroupYear? dbGiftingGroupYear = dbGroup.Years.FirstOrDefault(x => x.Year == _year);

        GiftingGroupYear giftingGroupYear = new();
        DateTime firstDayOfNextYear = new DateTime(_year + 1, 1, 1);

        var validGroupMembers = dbGroup.UserLinks
                .Where(x => x.DateDeleted == null && (x.DateArchived == null || x.DateArchived < firstDayOfNextYear));

        if (dbGiftingGroupYear != null)
        {
            Mapper.Map(dbGiftingGroupYear, giftingGroupYear);

            var missingGroupMembers = validGroupMembers
                .Where(x => giftingGroupYear.GroupMembers.Any(y => y.SantaUserId == x.SantaUserId) == false)
                .Select(x => Mapper.Map(x, new YearGroupUserBase()))
                .ToList();

            if (missingGroupMembers.Any())
            {
                missingGroupMembers.ForEach(x => x.Included = true);
                giftingGroupYear.GroupMembers.AddRange(missingGroupMembers);
            }
        }
        else
        {
            Mapper.Map(dbGroup, giftingGroupYear);

            giftingGroupYear.Year = _year;

            giftingGroupYear.GroupMembers = validGroupMembers
                .Select(x => Mapper.Map(x, new YearGroupUserBase()))
                .ToList();

            giftingGroupYear.GroupMembers.ForEach(x => x.Included = true);
        }

        foreach (var member in giftingGroupYear.GroupMembers)
        {
            await Send(new UnHashUserIdentificationAction(member));
        }

        if (string.IsNullOrEmpty(giftingGroupYear.CurrencyCode))
        {
            giftingGroupYear.CurrencyCode = CultureInfoExtensions.GetDefultCurrencyCode(dbGroup.CultureInfo);
        }

        if (string.IsNullOrEmpty(giftingGroupYear.CurrencySymbol))
        {
            giftingGroupYear.CurrencySymbol = CultureInfoExtensions.GetDefultCurrencySymbol(dbGroup.CultureInfo);
        }

        giftingGroupYear.GroupMembers = giftingGroupYear.GroupMembers.OrderBy(x => x.Forename).ThenBy(x => x.Surname).ToList();

        return giftingGroupYear;
    }
}
