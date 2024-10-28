using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Santa.Areas.GiftingGroup.Actions;

public class AddGroupDetailsForJoinerAction : BaseAction<IJoinGiftingGroup>
{
    public IJoinGiftingGroup Item { get; }

    public AddGroupDetailsForJoinerAction(IJoinGiftingGroup item)
    {
        Item = item;
    }

    protected override Task<bool> Handle()
    {
        EnsureSignedIn();

        Santa_GiftingGroup? dbGiftingGroup = null;

        if (Item.GroupId > 0)
        {
            dbGiftingGroup = DbContext.Santa_GiftingGroups.Where(x => x.Id == Item.GroupId).FirstOrDefault();

            if (dbGiftingGroup?.Name != Item.Name || dbGiftingGroup?.JoinerToken != Item.JoinerToken) // held on to an old ID?
            {                
                dbGiftingGroup = null;
                Item.GroupId = null;
            }
        }

        if (dbGiftingGroup == null)
        {
            dbGiftingGroup = DbContext.Santa_GiftingGroups
                .Where(x => x.Name == Item.Name && x.JoinerToken == Item.JoinerToken)
                .FirstOrDefault();
        }

        if (dbGiftingGroup == null)
        {
            return Task.FromResult(false);
        }        

        Global_User? dbGlobalUser = GetCurrentGlobalUser(g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);
        if (dbGlobalUser == null || dbGlobalUser.SantaUser == null)
        {
            throw new AccessDeniedException();
        }

        var existingGroupLinks = dbGlobalUser.SantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
            .Where(x => x.GiftingGroupId == dbGiftingGroup.Id);

        Item.GroupId = dbGiftingGroup.Id;
        Item.Description = dbGiftingGroup.Description;

        if (existingGroupLinks.Any())
        {
            Item.AlreadyMember = true;
            Item.GroupId = null;
        }

        var previousApplications = dbGlobalUser.SantaUser.GiftingGroupApplications
            .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
            .Where(x => x.GiftingGroupId == dbGiftingGroup.Id)
            .ToList();

        var blockedApplications = previousApplications
            .Where(x => x.Blocked);

        if (blockedApplications.Any())
        {
            Item.Blocked = true;
        }

        var pendingApplications = previousApplications
            .Where(x => x.Accepted == null);

        if (pendingApplications.Any())
        {
            Item.ApplicationPending = true;
        }

        return Task.FromResult(true);
    }
}
