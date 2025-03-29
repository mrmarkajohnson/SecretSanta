using Application.Areas.GiftingGroup.BaseModels;
using Application.Shared.BaseModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Mapping;

/// <summary>
/// This handles 'mappings' that don't work well in AutoMapper
/// </summary>
internal static class GiftingGroupManualMappings
{
    public static UserGiftingGroupYear ToUserGiftingGroupYear(this Santa_YearGroupUser dbYearGroupUser, IMapper mapper)
    {
        var dbGiftingGroupYear = dbYearGroupUser.GiftingGroupYear;

        return new UserGiftingGroupYear // AutoMapper just can't handle the Recipient with a ProjectTo as it may be null
        {
            GiftingGroupId = dbGiftingGroupYear.GiftingGroupId,
            GiftingGroupName = dbGiftingGroupYear.GiftingGroup.Name,
            GroupAdmin = dbGiftingGroupYear.GiftingGroup.UserLinks.First(u => dbYearGroupUser.SantaUserId == u.SantaUserId).GroupAdmin,
            Included = dbYearGroupUser.Included ?? false,
            Recipient = dbYearGroupUser.GivingToUserId > 0 
                ?  (mapper.Map<UserNamesBase>(dbYearGroupUser.GivingToUser).UnHash()) 
                : null,
            Limit = dbGiftingGroupYear.Limit,
            CurrencyCode = dbGiftingGroupYear.CurrencyCode ?? dbGiftingGroupYear.GiftingGroup.GetCurrencyCode(),
            CurrencySymbol = dbGiftingGroupYear.CurrencySymbol ?? dbGiftingGroupYear.GiftingGroup.GetCurrencySymbol(),
            Year = dbGiftingGroupYear.Year
        };
    }

    public static ManageUserGiftingGroupYear ToManageUserGiftingGroupYear(this Santa_YearGroupUser dbYearGroupUser, IMapper mapper)
    {
        UserGiftingGroupYear userGiftingGroupYear = dbYearGroupUser.ToUserGiftingGroupYear(mapper);
        var manageYear = mapper.Map<ManageUserGiftingGroupYear>(userGiftingGroupYear);
        SetPreviousYearDetails(manageYear, dbYearGroupUser.SantaUser, dbYearGroupUser.GiftingGroupYear.GiftingGroup, mapper);

        return manageYear;
    }

    public static void SetPreviousYearDetails(ManageUserGiftingGroupYear manageYear, Santa_User dbSantaUser, 
        Santa_GiftingGroup dbGiftingGroup, IMapper mapper)
    {
        manageYear.PreviousYearsRequired = dbSantaUser.PreviousYearsRequired(dbGiftingGroup, manageYear.Year);

        manageYear.OtherGroupMembers = dbGiftingGroup.UserLinks
            .Where(x => x.SantaUserId != dbSantaUser.Id)
            .Select(x => x.SantaUser)
            .AsQueryable().ProjectTo<IUserNamesBase>(mapper.ConfigurationProvider).ToList();

        manageYear.LastYearRecipientId = dbGiftingGroup.Recipient(dbSantaUser.Id, manageYear.Year - 1)?.GlobalUserId;
        manageYear.PreviousYearRecipientId = dbGiftingGroup.Recipient(dbSantaUser.Id, manageYear.Year - 2)?.GlobalUserId;
    }

    public static int PreviousYearsRequired(this Santa_User dbSantaUser, Santa_GiftingGroup dbGiftingGroup, int year)
    {
        if (dbSantaUser.GiftingGroupYears
            .Any(x => x.GiftingGroupYear.GiftingGroupId == dbGiftingGroup.Id && x.GiftingGroupYear.Year < year))
        {
            return 0;
        }

        int maxYears = Math.Max(Math.Min(year - dbGiftingGroup.FirstYear, 2), 0);

        if (maxYears == 2 && dbGiftingGroup.Recipient(dbSantaUser.Id, year - 2) != null)
        {
            maxYears = 1;
        }

        if (maxYears == 1 && dbGiftingGroup.Recipient(dbSantaUser.Id, year - 1) != null)
        {
            maxYears = 0;
        }

        return maxYears;
    }
}
