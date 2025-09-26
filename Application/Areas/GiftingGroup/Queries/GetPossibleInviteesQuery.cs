namespace Application.Areas.GiftingGroup.Queries;

public class GetPossibleInviteesQuery : BaseQuery<IQueryable<IVisibleUser>>
{
    public GetPossibleInviteesQuery(int giftingGroupKey)
    {
        _giftingGroupKey = giftingGroupKey;
    }

    private int _giftingGroupKey;

    protected override Task<IQueryable<IVisibleUser>> Handle()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var dbOtherGroupMembers = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.GiftingGroupKey == _giftingGroupKey)
            .Select(x => x.GiftingGroup)
            .SelectMany(y => y.Members)
            .Where(y => y.SantaUserKey != dbCurrentSantaUser.SantaUserKey);

        List<int> groupMemberKeys = dbOtherGroupMembers
            .Where(x => x.GiftingGroupKey == _giftingGroupKey)            
            .Select(x => x.SantaUserKey)
            .ToList();

        return dbOtherGroupMembers
            .Where(x => x.GiftingGroupKey != _giftingGroupKey)
            .Where(x => !groupMemberKeys.Contains(x.SantaUserKey);
    }
}
