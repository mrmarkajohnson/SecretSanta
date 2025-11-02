using Application.Areas.GiftingGroup.Queries.Internal;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Commands;

public abstract class GiftingGroupYearBaseCommand<TItem> : BaseCommand<TItem> where TItem : IHaveACalendarYear
{
    protected GiftingGroupYearBaseCommand(TItem item) : base(item)
    {
    }

    protected async Task<Santa_GiftingGroup> GetGiftingGroup(int giftingGroupKey, bool adminOnly)
    {
        Santa_GiftingGroupUser dbGiftingGroupLink = await Send(new GetGiftingGroupUserLinkQuery(giftingGroupKey, adminOnly));
        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;
        return dbGiftingGroup;
    }

    protected Santa_GiftingGroupYear GetOrCreateGiftingGroupYear(Santa_GiftingGroup dbGiftingGroup)
    {
        return GetOrCreateGiftingGroupYear(dbGiftingGroup, Item.CalendarYear);
    }

    protected Santa_YearGroupUser AddOrUpdateUserGroupYear(Santa_User dbSantaUser, int giftingGroupKey, 
        string giftingGroupName, bool included)
    {
        var dbGiftingGroupLink = dbSantaUser.GiftingGroupLinks
            .FirstOrDefault(x => x.DateDeleted == null && x.GiftingGroupKey == giftingGroupKey && x.GiftingGroup.DateDeleted == null);

        if (dbGiftingGroupLink == null)
            throw new NotFoundException($"Gifting Group '{giftingGroupName}'");

        return AddOrUpdateUserGroupYear(dbGiftingGroupLink, included);
    }

    private Santa_YearGroupUser AddOrUpdateUserGroupYear(Santa_GiftingGroupUser dbGiftingGroupLink, bool included)
    {
        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

        var dbGiftingGroupYear = dbGiftingGroup.Years
            .FirstOrDefault(y => y.CalendarYear == Item.CalendarYear);

        if (dbGiftingGroupYear == null)
        {
            dbGiftingGroupYear = CreateGiftingGroupYear(dbGiftingGroup, Item.CalendarYear);
        }

        Santa_User dbSantaUser = dbGiftingGroupLink.SantaUser;

        var dbYearGroupUser = AddOrUpdateUserGroupYear(dbGiftingGroupYear, included, dbSantaUser.SantaUserKey, 
            dbSantaUser.GlobalUser.DisplayName(), dbSantaUser);

        return dbYearGroupUser;
    }

    protected Santa_YearGroupUser AddOrUpdateUserGroupYear(Santa_GiftingGroupYear dbGiftingGroupYear, bool? included,
        int santaUserKey, string name, Santa_User? dbSantaUser = null)
    {
        Santa_YearGroupUser? dbYearGroupUser = dbGiftingGroupYear.Users.FirstOrDefault(x => x.SantaUserKey == santaUserKey);

        if (dbYearGroupUser == null)
        {
            dbSantaUser ??= DbContext.Santa_Users.FirstOrDefault(x => x.SantaUserKey == santaUserKey);

            if (dbSantaUser == null)
            {
                throw new ArgumentException($"User {name} could not be found."); // shouldn't happen
            }
            else
            {
                dbYearGroupUser = new Santa_YearGroupUser
                {
                    GiftingGroupYearKey = dbGiftingGroupYear.GiftingGroupYearKey,
                    GiftingGroupYear = dbGiftingGroupYear,
                    SantaUserKey = dbSantaUser.SantaUserKey,
                    SantaUser = dbSantaUser,
                    Included = included
                };

                dbGiftingGroupYear.Users.Add(dbYearGroupUser);
            }
        }
        else
        {
            dbYearGroupUser.Included = included;
        }

        return dbYearGroupUser;
    }
}
