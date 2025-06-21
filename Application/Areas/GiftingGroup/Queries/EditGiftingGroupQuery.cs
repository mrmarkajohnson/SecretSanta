using Application.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Queries;

public sealed class EditGiftingGroupQuery : GiftingGroupBaseQuery<IGiftingGroup>
{
    private readonly int _giftingGroupKey;

    public EditGiftingGroupQuery(int giftingGroupKey)
    {
        _giftingGroupKey = giftingGroupKey;
    }

    protected async override Task<IGiftingGroup> Handle()
    {
        if (_giftingGroupKey == 0)
        {
            return new CoreGiftingGroup();
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await GetGiftingGroupUserLink(_giftingGroupKey, true);
        return Mapper.Map<IGiftingGroup>(dbGiftingGroupLink.GiftingGroup);
    }
}
