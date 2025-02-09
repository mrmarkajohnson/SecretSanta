using Application.Areas.GiftingGroup.BaseModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Global.Settings;

namespace Application.Areas.GiftingGroup.Queries;

public class GetUserGiftingGroupYearQuery : BaseQuery<IUserGiftingGroupYear>
{
    public int GroupId { get; }

    public GetUserGiftingGroupYearQuery(int groupId)
    {
        GroupId = groupId;
    }

    protected async override Task<IUserGiftingGroupYear> Handle()
    {
        IQueryable<IUserGiftingGroupYear> GiftingGroupYears = await Send(new UserGiftingGroupYearsQuery());
        IUserGiftingGroupYear? GiftingGroupYear = GiftingGroupYears.FirstOrDefault(x => x.GiftingGroupId == GroupId);

        if (GiftingGroupYear == null) // not created yet
        {
            Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

            var dbGiftingGroupLink = dbSantaUser.GiftingGroupLinks
                    .FirstOrDefault(x => x.DateDeleted == null && x.GiftingGroupId == GroupId && x.GiftingGroup.DateDeleted == null);

            if (dbGiftingGroupLink == null)
                throw new NotFoundException("Gifting Group");

            Santa_GiftingGroup dbGroup = dbGiftingGroupLink.GiftingGroup;
            System.Globalization.CultureInfo? groupCultureInfo = GlobalSettings.AvailableCultures.FirstOrDefault(x => x.Name == dbGroup.CultureInfo);

            GiftingGroupYear = new UserGiftingGroupYear
            {
                GiftingGroupId = dbGiftingGroupLink.GiftingGroupId,
                GiftingGroupName = dbGroup.Name,
                CurrencyCode = dbGroup.GetCurrencyCode(),
                CurrencySymbol = dbGroup.GetCurrencySymbol(),
                Year = DateTime.Today.Year,
                GroupAdmin = dbGiftingGroupLink.GroupAdmin
            };
        }

        return GiftingGroupYear;
    }
}
