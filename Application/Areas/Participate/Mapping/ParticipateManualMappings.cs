using Application.Areas.Participate.BaseModels;
using Application.Shared.BaseModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.Participate;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.Participate.Mapping;

/// <summary>
/// This handles 'mappings' that don't work well in AutoMapper
/// </summary>
internal static class ParticipateManualMappings
{
    public static UserGiftingGroupYear ToUserGiftingGroupYear(this Santa_YearGroupUser dbYearGroupUser, IMapper mapper)
    {
        var dbGiftingGroupYear = dbYearGroupUser.GiftingGroupYear;

        return new UserGiftingGroupYear // AutoMapper just can't handle the Recipient with a ProjectTo as it may be null
        {
            GiftingGroupKey = dbGiftingGroupYear.GiftingGroupKey,
            GiftingGroupName = dbGiftingGroupYear.GiftingGroup.Name,
            MemberStatus = dbGiftingGroupYear.GiftingGroup.UserLinks.First(u => dbYearGroupUser.SantaUserKey == u.SantaUserKey).GroupAdmin
                ? GroupMemberStatus.Admin 
                : GroupMemberStatus.Joined,
            Included = dbYearGroupUser.Included ?? false,
            Recipient = dbYearGroupUser.RecipientSantaUserKey > 0
                ? mapper.Map<UserNamesBase>(dbYearGroupUser.RecipientSantaUser).UnHash()
                : null,
            Limit = dbGiftingGroupYear.Limit,
            CurrencyCode = dbGiftingGroupYear.CurrencyCode ?? dbGiftingGroupYear.GiftingGroup.GetCurrencyCode(),
            CurrencySymbol = dbGiftingGroupYear.CurrencySymbol ?? dbGiftingGroupYear.GiftingGroup.GetCurrencySymbol(),
            CalendarYear = dbGiftingGroupYear.CalendarYear
        };
    }

    public static ManageUserGiftingGroupYear ToManageUserGiftingGroupYear(this Santa_YearGroupUser dbYearGroupUser, IMapper mapper)
    {
        UserGiftingGroupYear userGiftingGroupYear = dbYearGroupUser.ToUserGiftingGroupYear(mapper);
        var manageYear = mapper.Map<ManageUserGiftingGroupYear>(userGiftingGroupYear);
        SetPreviousYearDetails(manageYear, dbYearGroupUser.SantaUser, dbYearGroupUser.GiftingGroupYear.GiftingGroup, mapper);

        return manageYear;
    }

    public static void SetPreviousYearDetails(IManageUserGiftingGroupYear manageYear, Santa_User dbSantaUser,
        Santa_GiftingGroup dbGiftingGroup, IMapper mapper)
    {
        manageYear.PreviousYearsRequired = dbSantaUser.PreviousYearsRequired(dbGiftingGroup, manageYear.CalendarYear);

        manageYear.OtherGroupMembers = dbGiftingGroup.UserLinks
            .Where(x => x.SantaUserKey != dbSantaUser.SantaUserKey)
            .Select(x => x.SantaUser)
            .AsQueryable()
            .ProjectTo<IUserNamesBase>(mapper.ConfigurationProvider, new { UserKeysForVisibleEmail = dbSantaUser.UserKeysForVisibleEmail() })
            .ToList();

        manageYear.LastRecipientUserId = dbGiftingGroup.Recipient(dbSantaUser.SantaUserKey, manageYear.CalendarYear - 1)?.GlobalUserId;
        manageYear.PreviousRecipientUserId = dbGiftingGroup.Recipient(dbSantaUser.SantaUserKey, manageYear.CalendarYear - 2)?.GlobalUserId;
    }

    public static int PreviousYearsRequired(this Santa_User dbSantaUser, Santa_GiftingGroup dbGiftingGroup, int year)
    {
        if (dbSantaUser.GiftingGroupYears
            .Any(x => x.GiftingGroupYear.GiftingGroupKey == dbGiftingGroup.GiftingGroupKey && x.GiftingGroupYear.CalendarYear < year))
        {
            return 0;
        }

        int maxYears = Math.Max(Math.Min(year - dbGiftingGroup.FirstYear, 2), 0);

        if (maxYears == 2 && dbGiftingGroup.Recipient(dbSantaUser.SantaUserKey, year - 2) != null)
        {
            maxYears = 1;
        }

        if (maxYears == 1 && dbGiftingGroup.Recipient(dbSantaUser.SantaUserKey, year - 1) != null)
        {
            maxYears = 0;
        }

        return maxYears;
    }
}
