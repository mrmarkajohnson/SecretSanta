using Application.Areas.GiftingGroup.Commands;
using Global.Abstractions.Areas.Participate;

namespace Application.Areas.Participate.Commands;

public sealed class ParticipateInYearCommand<TItem> : GiftingGroupYearBaseCommand<TItem> where TItem : IManageUserGiftingGroupYear
{
    public ParticipateInYearCommand(TItem item) : base(item)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        Santa_YearGroupUser dbYearGroupUser = AddOrUpdateUserGroupYear(dbSantaUser, Item.GiftingGroupKey, Item.GiftingGroupName, Item.Included);
        Santa_GiftingGroup dbGiftingGroup = dbYearGroupUser.GiftingGroupYear.GiftingGroup;

        SetPreviousYearRecipient(dbSantaUser, dbGiftingGroup, Item.LastRecipientUserId, Item.CalendarYear - 1);
        SetPreviousYearRecipient(dbSantaUser, dbGiftingGroup, Item.PreviousRecipientUserId, Item.CalendarYear - 2);

        return await SaveAndReturnSuccess();
    }

    private void SetPreviousYearRecipient(Santa_User dbSantaUser, Santa_GiftingGroup dbGiftingGroup, string? recipientId, int oldYear)
    {
        if (string.IsNullOrWhiteSpace(recipientId) || dbGiftingGroup.Recipient(dbSantaUser.SantaUserKey, oldYear) != null)
            return; // TODO: Return a validation failure

        var dbMatchedGroupUser = dbGiftingGroup.UserLinks.FirstOrDefault(x => x.SantaUser.GlobalUserId == recipientId);
        if (dbMatchedGroupUser == null)
            return; // TODO: Return a validation failure

        Santa_GiftingGroupYear? dbOldYear = dbGiftingGroup.Years.FirstOrDefault(x => x.CalendarYear == oldYear);

        if (dbOldYear == null)
        {
            dbOldYear = new Santa_GiftingGroupYear
            {
                CalendarYear = oldYear,
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
