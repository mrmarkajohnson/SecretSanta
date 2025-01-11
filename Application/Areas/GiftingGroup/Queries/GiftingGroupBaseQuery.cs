using Application.Shared.Requests;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries;

public abstract class GiftingGroupBaseQuery<TItem> : BaseQuery<TItem>
{
    protected async Task<Santa_GiftingGroupUser> GetGiftingGroupUserLink(int groupId, bool adminOnly)
    {
        Global_User dbCurrentUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
        Santa_User? dbSantaUser = dbCurrentUser.SantaUser;

        if (dbSantaUser != null)
        {
            Santa_GiftingGroupUser? dbGiftingGroupLink = dbSantaUser.GiftingGroupLinks
                .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
                .FirstOrDefault(x => x.GiftingGroupId == groupId);

            if (dbGiftingGroupLink != null)
            {
                if (!adminOnly || dbGiftingGroupLink.GroupAdmin)
                {
                    await Task.CompletedTask;
                    return dbGiftingGroupLink;
                }
                else
                {
                    throw new AccessDeniedException();
                }
            }
        }

        throw new NotFoundException("Gifting Group");
    }
}
