namespace Application.Santa.Areas.GiftingGroup.Queries.Internal;

internal class GetGiftingGroupUserLinkQuery : GiftingGroupBaseQuery<Santa_GiftingGroupUser>
{
    private readonly int _groupId;
    private readonly bool _adminOnly;

    public GetGiftingGroupUserLinkQuery(int groupId, bool adminOnly)
    {
        _groupId = groupId;
        _adminOnly = adminOnly;
    }

    protected override Task<Santa_GiftingGroupUser> Handle()
    {
        return GetGiftingGroupUserLink(_groupId, _adminOnly);
    }
}
