using Global.Abstractions.Areas.GiftingGroup;
using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Commands;

public sealed class ParticipateInYearCommand<TItem> : GiftingGroupYearBaseCommand<TItem> where TItem : IManageUserGiftingGroupYear
{
    public ParticipateInYearCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        var dbGiftingGroupLink = dbSantaUser.GiftingGroupLinks
                .FirstOrDefault(x => x.DateDeleted == null && x.GiftingGroupKey == Item.GiftingGroupKey && x.GiftingGroup.DateDeleted == null);

        if (dbGiftingGroupLink == null)
            throw new NotFoundException($"Gifting Group '{Item.GiftingGroupName}'");

        Santa_GiftingGroup dbGiftingGroup = dbGiftingGroupLink.GiftingGroup;

        var dbGiftingGroupYear = dbGiftingGroup.Years
            .FirstOrDefault(y => y.Year == Item.Year);

        if (dbGiftingGroupYear == null)
        {
            dbGiftingGroupYear = CreateGiftingGroupYear(dbGiftingGroup);
        }

        AddOrUpdateUserGroupYear(dbGiftingGroupYear, Item.Included, dbSantaUser.SantaUserKey, dbSantaUser.GlobalUser.FullName(), dbSantaUser);

        SetPreviousYearRecipient(dbSantaUser, dbGiftingGroup, Item.LastRecipientUserId, Item.Year - 1);
        SetPreviousYearRecipient(dbSantaUser, dbGiftingGroup, Item.PreviousRecipientUserId, Item.Year - 2);

        return await SaveAndReturnSuccess();
    }

    private void SetPreviousYearRecipient(Santa_User dbSantaUser, Santa_GiftingGroup dbGiftingGroup, string? recipientId, int oldYear)
    {
        if (string.IsNullOrWhiteSpace(recipientId) || dbGiftingGroup.Recipient(dbSantaUser.SantaUserKey, oldYear) != null)
            return; // TODO: Return a validation failure

        var dbMatchedGroupUser = dbGiftingGroup.UserLinks.FirstOrDefault(x => x.SantaUser.GlobalUserId == recipientId);
        if (dbMatchedGroupUser == null)
            return; // TODO: Return a validation failure

        Santa_GiftingGroupYear? dbOldYear = dbGiftingGroup.Years.FirstOrDefault(x => x.Year == oldYear);

        if (dbOldYear == null)
        {
            dbOldYear = new Santa_GiftingGroupYear
            {
                Year = oldYear,
                CurrencyCode = dbGiftingGroup.GetCurrencyCode(),
                CurrencySymbol = dbGiftingGroup.GetCurrencySymbol(),
                GiftingGroupKey = dbGiftingGroup.GiftingGroupKey,
                GiftingGroup = dbGiftingGroup
            };

            DbContext.ChangeTracker.DetectChanges();
        }

        var dbOldYearUser = dbOldYear.Users.FirstOrDefault(x => x.SantaUserKey == dbSantaUser.SantaUserKey);

        if (dbOldYearUser == null)
        {
            dbOldYearUser = new Santa_YearGroupUser
            {
                GiftingGroupYearKey = dbOldYear.GiftingGroupYearKey,
                GiftingGroupYear = dbOldYear,
                SantaUserKey = dbSantaUser.SantaUserKey,
                SantaUser = dbSantaUser
            };

            DbContext.ChangeTracker.DetectChanges();
        }

        dbOldYearUser.Included = true;
        dbOldYearUser.RecipientSantaUserKey = dbMatchedGroupUser.SantaUserKey;
        dbOldYearUser.RecipientSantaUser = dbMatchedGroupUser.SantaUser;
    }
}
