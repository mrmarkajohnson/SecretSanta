using Application.Areas.GiftingGroup.BaseModels;
using Application.Areas.GiftingGroup.Mapping;
using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Data.Migrations;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Global.Settings;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class ManageUserGiftingGroupYearQuery : BaseQuery<IManageUserGiftingGroupYear>
{
    public int GiftingGroupKey { get; }
    public int Year { get; }

    public ManageUserGiftingGroupYearQuery(int giftingGroupKey, int? year = null)
    {
        GiftingGroupKey = giftingGroupKey;
        Year = year ?? DateTime.Today.Year;
    }

    protected override Task<IManageUserGiftingGroupYear> Handle()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);
        IQueryable<IUserGiftingGroupYear> userGroups = new List<IUserGiftingGroupYear>().AsQueryable();

        ICollection<Santa_GiftingGroupUser> dbGroupLinks = dbSantaUser.GiftingGroupLinks;

        if (dbGroupLinks?.Any() != true)
            throw new NotFoundException("Gifting Group");

        var dbActiveLinks = dbGroupLinks
                .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null);

        Santa_GiftingGroupUser? dbGiftingGroupLink = dbSantaUser.GiftingGroupLinks
            .FirstOrDefault(x => x.GiftingGroupKey == GiftingGroupKey);

        if (dbGiftingGroupLink == null)
            throw new NotFoundException("Gifting Group");

        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

        Santa_GiftingGroupYear? dbYear = dbGiftingGroup.Years
            .Where(x => x.Year == Year)
            .FirstOrDefault();

        Santa_YearGroupUser? dbYearGroupUser = dbYear?.Users.FirstOrDefault(u => u.SantaUserKey == dbSantaUser.SantaUserKey);

        IManageUserGiftingGroupYear? manageYear = dbYearGroupUser?.ToManageUserGiftingGroupYear(Mapper);

        if (manageYear == null) // not created yet
        {
            manageYear = new ManageUserGiftingGroupYear
            {
                GiftingGroupKey = dbGiftingGroupLink.GiftingGroupKey,
                GiftingGroupName = dbGiftingGroup.Name,
                GroupAdmin = dbGiftingGroupLink.GroupAdmin,
                Limit = dbYear?.Limit,
                CurrencyCode = dbYear?.CurrencyCode ?? dbGiftingGroup.GetCurrencyCode(),
                CurrencySymbol = dbYear?.CurrencySymbol ?? dbGiftingGroup.GetCurrencySymbol(),
                Year = Year
            };

            GiftingGroupManualMappings.SetPreviousYearDetails(manageYear, dbSantaUser, dbGiftingGroup, Mapper);
        }

        return Task.FromResult(manageYear);
    }
}
