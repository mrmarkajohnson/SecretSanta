namespace Application.Areas.GiftingGroup.Queries.Internal;

internal class GetGiftingGroupUserLinkQuery : GiftingGroupBaseQuery<Santa_GiftingGroupUser>
{
    private readonly int _giftingGroupKey;
    private readonly bool _adminOnly;

    public GetGiftingGroupUserLinkQuery(int giftingGroupKey, bool adminOnly)
    {
        _giftingGroupKey = giftingGroupKey;
        _adminOnly = adminOnly;
    }

    protected override Task<Santa_GiftingGroupUser> Handle()
    {
        return GetGiftingGroupUserLink(_giftingGroupKey, _adminOnly);
    }
}
