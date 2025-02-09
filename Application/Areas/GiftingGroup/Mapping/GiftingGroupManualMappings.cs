using Application.Areas.GiftingGroup.BaseModels;
using Application.Shared.BaseModels;
using AutoMapper;

namespace Application.Areas.GiftingGroup.Mapping;

/// <summary>
/// This handles 'mappings' that don't work well in AutoMapper
/// </summary>
internal static class GiftingGroupManualMappings
{
    public static UserGiftingGroupYear ToUserGiftingGroupYear(this Santa_YearGroupUser dbYearGroupUser, IMapper mapper)
    {
        var dbGiftingGroupYear = dbYearGroupUser.Year;

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
}
