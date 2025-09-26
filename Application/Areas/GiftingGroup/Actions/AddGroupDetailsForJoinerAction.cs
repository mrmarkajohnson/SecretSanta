using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Actions;

public sealed class AddGroupDetailsForJoinerAction : BaseAction<IJoinGiftingGroup>
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

        if (Item.GiftingGroupKey > 0)
        {
            dbGiftingGroup = DbContext.Santa_GiftingGroups.Where(x => x.GiftingGroupKey == Item.GiftingGroupKey).FirstOrDefault();

            if (dbGiftingGroup?.Name != Item.Name || dbGiftingGroup?.JoinerToken != Item.JoinerToken) // held on to an old ID?
            {
                dbGiftingGroup = null;
                Item.GiftingGroupKey = null;
            }
        }

        if (dbGiftingGroup == null)
        {
            dbGiftingGroup = DbContext.Santa_GiftingGroups
                .FirstOrDefault(x => x.Name == Item.Name && x.JoinerToken == Item.JoinerToken);
        }

        if (dbGiftingGroup == null)
        {
            return FailureActionResult();
        }

        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var existingGroupLinks = dbCurrentSantaUser.GiftingGroupLinks
            .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
            .Where(x => x.GiftingGroupKey == dbGiftingGroup.GiftingGroupKey);

        Item.GiftingGroupKey = dbGiftingGroup.GiftingGroupKey;
        Item.Description = dbGiftingGroup.Description;

        if (existingGroupLinks.Any())
        {
            Item.AlreadyMember = true;
            Item.GiftingGroupKey = null;
        }

        var previousApplications = dbCurrentSantaUser.GiftingGroupApplications
            .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
            .Where(x => x.GiftingGroupKey == dbGiftingGroup.GiftingGroupKey)
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

        return SuccessActionResult();
    }
}
