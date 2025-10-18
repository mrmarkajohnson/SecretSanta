using Application.Areas.GiftingGroup.Queries.Internal;
using AutoMapper.QueryableExtensions;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.GiftingGroup.Queries;

public class GetGiftingGroupMembersQuery : GiftingGroupBaseQuery<IQueryable<IGroupMember>>
{
    private readonly int _giftingGroupKey;
    private readonly OtherGroupMembersType _memberListType;
    private readonly Guid? _invitationGuid;

    public GetGiftingGroupMembersQuery(int giftingGroupKey, OtherGroupMembersType memberListType, Guid? invitationGuid = null)
    {
        _giftingGroupKey = giftingGroupKey;
        _memberListType = memberListType;
        _invitationGuid = invitationGuid;
    }

    protected async override Task<IQueryable<IGroupMember>> Handle()
    {
        if (_giftingGroupKey == 0)
        {
            throw new NotFoundException("Gifting Group");
        }

        Santa_GiftingGroupUser? dbGiftingGroupLink = _memberListType == OtherGroupMembersType.ReviewInvitation
            ? null
            : await GetGiftingGroupUserLink(_giftingGroupKey, false);
        
        Santa_GiftingGroup? dbGiftingGroup = _memberListType == OtherGroupMembersType.ReviewInvitation
            ? DbContext.Santa_GiftingGroups.FirstOrDefault(x => x.GiftingGroupKey == _giftingGroupKey)
            : dbGiftingGroupLink?.GiftingGroup;

        if (dbGiftingGroup == null)
        {
            return new List<IGroupMember>().AsQueryable();
        }

        IList<int> userKeysForVisibleEmail = [];

        IEnumerable<Santa_GiftingGroupUser> groupMembers = dbGiftingGroup.Members
            .Where(x => x.DateDeleted == null && x.DateArchived == null);
        
        if (SignedIn())
        {
            Santa_User dbCurrentSantaUser = GetCurrentSantaUser();
            groupMembers = groupMembers.Where(x => x.SantaUserKey != dbCurrentSantaUser.SantaUserKey).ToList();
            userKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail();
        }

        if (_memberListType == OtherGroupMembersType.ReviewInvitation && _invitationGuid != null)
        {
            var dbInvitation = await Send(new GetInvitationEntityQuery(_invitationGuid.Value));
            if (dbInvitation != null)
            {
                userKeysForVisibleEmail.Add(dbInvitation.FromSantaUserKey);
            }
        }

        var result = groupMembers
            .AsQueryable()
            .ProjectTo<IGroupMember>(Mapper.ConfigurationProvider, new { UserKeysForVisibleEmail = userKeysForVisibleEmail })
            .ToList();

        foreach (var member in result)
        {
            member.UnHash();

            if (dbGiftingGroupLink?.GroupAdmin == true)
            {
                member.ShowEmail = true;
            }
        }

        return result.AsQueryable();
    }
}
