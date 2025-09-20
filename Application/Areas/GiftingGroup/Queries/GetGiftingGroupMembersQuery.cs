using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries;

public class GetGiftingGroupMembersQuery : GiftingGroupBaseQuery<IQueryable<IGroupMember>>
{
    private readonly int _giftingGroupKey;
    private readonly bool _otherMembersOnly;

    public GetGiftingGroupMembersQuery(int giftingGroupKey, bool otherMembersOnly)
    {
        _giftingGroupKey = giftingGroupKey;
        _otherMembersOnly = otherMembersOnly;
    }

    protected async override Task<IQueryable<IGroupMember>> Handle()
    {
        if (_giftingGroupKey == 0)
        {
            throw new NotFoundException("Gifting Group");
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await GetGiftingGroupUserLink(_giftingGroupKey, false);
        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

        var groupMembers = dbGiftingGroup.Members
            .Where(x => x.DateDeleted == null && x.DateArchived == null);

        if (_otherMembersOnly)
        {
            Santa_User dbCurrentSantaUser = GetCurrentSantaUser();
            groupMembers = groupMembers.Where(x => x.SantaUserKey != dbCurrentSantaUser.SantaUserKey).ToList();
        }

        var result = groupMembers
            .AsQueryable()
            .ProjectTo<IGroupMember>(Mapper.ConfigurationProvider)
            .ToList();

        foreach (var member in result)
        {
            member.UnHash();

            if (dbGiftingGroupLink.GroupAdmin)
            {
                member.ShowEmail = true;
            }
        }

        return result.AsQueryable();
    }
}
