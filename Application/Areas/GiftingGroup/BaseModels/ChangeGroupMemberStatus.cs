namespace Application.Areas.GiftingGroup.BaseModels;

public sealed class ChangeGroupMemberStatus
{
	public ChangeGroupMemberStatus(int giftingGroupKey, int santaUserKey)
	{
        GiftingGroupKey = giftingGroupKey;
        SantaUserKey = santaUserKey;
    }

    public int GiftingGroupKey { get; }
    public int SantaUserKey { get; }
}
