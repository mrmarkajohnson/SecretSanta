using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class GetUserGiftingGroupsQuery : BaseQuery<IList<IUserGiftingGroup>>
{
    public GetUserGiftingGroupsQuery()
    {
    }

    protected override Task<IList<IUserGiftingGroup>> Handle()
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser!.GiftingGroupLinks);
        IList<IUserGiftingGroup> userGroups = new List<IUserGiftingGroup>();

        if (dbCurrentUser?.SantaUser != null)
        {
            userGroups = dbCurrentUser.SantaUser.GiftingGroupLinks
                .Where(GroupUserExpressions.IsActive(true))
                .AsQueryable()
                .ProjectTo<IUserGiftingGroup>(Mapper.ConfigurationProvider)
                .ToList();
        }

        return Result(userGroups);
    }
}
