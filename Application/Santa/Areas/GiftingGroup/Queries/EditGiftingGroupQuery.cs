using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.GiftingGroup.Queries;

public class EditGiftingGroupQuery : BaseQuery<IGiftingGroup>
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
        
        Global_User? dbGlobalUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);

        if (dbGlobalUser != null)
        {
            Santa_User? dbSantaUser = dbGlobalUser.SantaUser;

            if (dbSantaUser != null)
            {
                Santa_GiftingGroupUser? dbGiftingGroupLink = dbSantaUser.GiftingGroupLinks
                    .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
                    .FirstOrDefault(x => x.GiftingGroupId == _groupId);

                if (dbGiftingGroupLink != null)
                {
                    if (dbGiftingGroupLink.GroupAdmin)
                    {
                        await Task.CompletedTask;
                        return Mapper.Map<IGiftingGroup>(dbGiftingGroupLink.GiftingGroup);
                    }
                    else
                    {
                        throw new AccessDeniedException();
                    }
                }
            }
        }

        throw new NotFoundException("Gifting Group");
    }
}
