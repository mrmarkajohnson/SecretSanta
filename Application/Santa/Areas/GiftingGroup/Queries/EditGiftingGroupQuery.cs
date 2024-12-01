using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.GiftingGroup.Queries;

public class EditGiftingGroupQuery : GiftingGroupBaseQuery<IGiftingGroup>
{
    private readonly int _groupId;

    public EditGiftingGroupQuery(int groupId)
    {
        _groupId = groupId;
    }

    protected async override Task<IGiftingGroup> Handle()
    {
        if (_groupId == 0)
        {
            return new CoreGiftingGroup();
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await GetGiftingGroupUserLink(_groupId, true);
        return Mapper.Map<IGiftingGroup>(dbGiftingGroupLink.GiftingGroup);
    }

    
}
