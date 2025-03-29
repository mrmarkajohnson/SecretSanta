using Application.Areas.GiftingGroup.BaseModels;
using Application.Areas.GiftingGroup.Mapping;
using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Data.Migrations;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Global.Settings;

namespace Application.Areas.GiftingGroup.Queries;

public class ManageUserGiftingGroupYearQuery : BaseQuery<IManageUserGiftingGroupYear>
{
    public int GroupId { get; }
    public int Year { get; }

    public ManageUserGiftingGroupYearQuery(int groupId, int? year = null)
    {
        GroupId = groupId;
        Year = year ?? DateTime.Today.Year;
    }

    protected async override Task<IManageUserGiftingGroupYear> Handle()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);
        IQueryable<IUserGiftingGroupYear> userGroups = new List<IUserGiftingGroupYear>().AsQueryable();

        ICollection<Santa_GiftingGroupUser> dbGroupLinks = dbSantaUser.GiftingGroupLinks;

        if (dbGroupLinks?.Any() != true)
            throw new NotFoundException("Gifting Group");

        var dbActiveLinks = dbGroupLinks
                .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null);

        Santa_GiftingGroupUser? dbGiftingGroupLink = dbSantaUser.GiftingGroupLinks
            .FirstOrDefault(x => x.GiftingGroupId == GroupId);

        if (dbGiftingGroupLink == null)
            throw new NotFoundException("Gifting Group");

        Santa_GiftingGroup dbGroup = dbGiftingGroupLink.GiftingGroup;

        Santa_GiftingGroupYear? dbYear = dbGroup.Years
            .Where(x => x.Year == Year)
            .FirstOrDefault();

        Santa_YearGroupUser? dbYearGroupUser = dbYear?.Users.FirstOrDefault(u => u.SantaUserId == dbSantaUser.Id);

        ManageUserGiftingGroupYear? manageYear = dbYearGroupUser?.ToManageUserGiftingGroupYear(Mapper);

        if (manageYear == null) // not created yet
        {
            manageYear = new ManageUserGiftingGroupYear
            {
                GiftingGroupId = dbGiftingGroupLink.GiftingGroupId,
                GiftingGroupName = dbGroup.Name,
                GroupAdmin = dbGiftingGroupLink.GroupAdmin,
                Limit = dbYear?.Limit,
                CurrencyCode = dbYear?.CurrencyCode ?? dbGroup.GetCurrencyCode(),
                CurrencySymbol = dbYear?.CurrencySymbol ?? dbGroup.GetCurrencySymbol(),
                Year = Year
            };

            GiftingGroupManualMappings.SetPreviousYearDetails(manageYear, dbSantaUser, dbGroup, Mapper);
        }

        return manageYear;
    }
}
