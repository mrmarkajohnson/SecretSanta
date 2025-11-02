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

            if (_memberListType != OtherGroupMembersType.MessageRecipients) // for messages this is stripped out later, as it uses the information on the current user, too
            {
                groupMembers = groupMembers.Where(x => x.SantaUserKey != dbCurrentSantaUser.SantaUserKey).ToList();
            }

            userKeysForVisibleEmail = dbCurrentSantaUser.UserKeysForVisibleEmail();
        }

        if (_memberListType == OtherGroupMembersType.ReviewInvitation && _invitationGuid != null)
        {
            try
            {
                var dbInvitation = await Send(new GetInvitationEntitySavingQuery(_invitationGuid.Value));
                if (dbInvitation != null)
                {
                    userKeysForVisibleEmail.Add(dbInvitation.FromSantaUserKey);
                }
            }
            catch { }
        }

        object parameters = new { UserKeysForVisibleEmail = userKeysForVisibleEmail };

        var result = groupMembers
            .AsQueryable()
            .ProjectTo<IGroupMember>(Mapper.ConfigurationProvider, parameters)
            .ToList();

        List<int> santaUserKeys = result.Select(x => x.SantaUserKey).ToList();

        var invitees = dbGiftingGroup.Invitations
            .Where(x => x.DateArchived == null)
            .Where(x => x.ToSantaUserKey != null && !santaUserKeys.Contains(x.ToSantaUserKey.Value))
            .AsQueryable()
            .ProjectTo<IGroupMember>(Mapper.ConfigurationProvider, parameters)
            .ToList();

        result.AddRange(invitees);

        var applicants = dbGiftingGroup.MemberApplications
            .Where(x => x.DateArchived == null && x.DateDeleted == null && x.Accepted == null)
            .Where(x => !santaUserKeys.Contains(x.SantaUserKey))
            .AsQueryable()
            .ProjectTo<IGroupMember>(Mapper.ConfigurationProvider, parameters)
            .ToList();

        result.AddRange(applicants);

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
