using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;

namespace Application.Santa.Areas.GiftingGroup.Queries;

public class GetUserGiftingGroupsQuery : BaseQuery<IList<IUserGiftingGroup>>
{
    public GetUserGiftingGroupsQuery()
    {
    }

    protected override Task<IList<IUserGiftingGroup>> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);

        ICollection<Santa_GiftingGroupUser>? dbGroupLinks = dbCurrentUser?.SantaUser?.GiftingGroupLinks;
        IList<IUserGiftingGroup> userGroups = new List<IUserGiftingGroup>();

        if (dbGroupLinks?.Any() == true)
        {
            userGroups = dbGroupLinks
                .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null)
                .Select(x => new UserGiftingGroup 
                { 
                    GiftingGroupId = x.GiftingGroupId, 
                    GroupName = x.GiftingGroup.Name, 
                    GroupAdmin = x.GroupAdmin,
                    NewApplications = x.GroupAdmin ? x.GiftingGroup.MemberApplications.Where(x => !x.Blocked && x.ResponseByUserId == null).Count() : 0,
                })
                .ToList<IUserGiftingGroup>();
        }

        return Task.FromResult(userGroups);
    }
}
