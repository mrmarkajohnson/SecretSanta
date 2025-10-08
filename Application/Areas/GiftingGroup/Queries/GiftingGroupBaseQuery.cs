using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries;

public abstract class GiftingGroupBaseQuery<TItem> : BaseQuery<TItem>
{
    protected async Task<Santa_GiftingGroupUser> GetGiftingGroupUserLink(int giftingGroupKey, bool adminOnly)
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        Santa_GiftingGroupUser? dbGiftingGroupLink = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
            .FirstOrDefault(x => x.GiftingGroupKey == giftingGroupKey);

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

        throw new NotFoundException("Gifting Group");
    }
}
