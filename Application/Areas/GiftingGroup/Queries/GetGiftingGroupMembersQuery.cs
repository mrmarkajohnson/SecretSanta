using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries;

public class GetGiftingGroupMembersQuery : GiftingGroupBaseQuery<IEnumerable<IGroupMember>>
{
    private readonly int _giftingGroupKey;

    public GetGiftingGroupMembersQuery(int giftingGroupKey)
    {
        _giftingGroupKey = giftingGroupKey;
    }

    protected async override Task<IEnumerable<IGroupMember>> Handle()
    {
        if (_giftingGroupKey == 0)
        {
            throw new NotFoundException("Gifting Group");
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await GetGiftingGroupUserLink(_giftingGroupKey, false);
        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

        return dbGiftingGroup.Members
            .Where(x => x.DateDeleted == null && x.DateArchived == null)
            .AsQueryable()
            .ProjectTo<IGroupMember>(Mapper.ConfigurationProvider);
    }
}
