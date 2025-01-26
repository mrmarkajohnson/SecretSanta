using Application.Shared.Requests;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;
using Microsoft.EntityFrameworkCore;

namespace Application.Areas.GiftingGroup.Queries;

public class GetUserGiftingGroupsQuery : BaseQuery<IList<IUserGiftingGroup>>
{
    public GetUserGiftingGroupsQuery()
    {
    }

    protected override Task<IList<IUserGiftingGroup>> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
        IList<IUserGiftingGroup> userGroups = new List<IUserGiftingGroup>();

        if (dbCurrentUser?.SantaUser != null)
        {
            ICollection<Santa_GiftingGroupUser> dbGroupLinks = dbCurrentUser.SantaUser.GiftingGroupLinks;            

            if (dbGroupLinks?.Any() == true)
            {
                userGroups = dbGroupLinks
                    .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null)
                    .AsQueryable()
                    .ProjectTo<IUserGiftingGroup>(Mapper.ConfigurationProvider)
                    .ToList();
            }
        }

        return Task.FromResult(userGroups);
    }
}
