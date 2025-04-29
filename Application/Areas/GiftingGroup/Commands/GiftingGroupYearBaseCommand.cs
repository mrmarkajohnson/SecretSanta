using Application.Shared.Requests;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Commands;

public abstract class GiftingGroupYearBaseCommand<TItem> : BaseCommand<TItem> where TItem : IGiftingGroupYearBase
{
    protected GiftingGroupYearBaseCommand(TItem item) : base(item)
    {
    }

    protected Santa_GiftingGroupYear CreateGiftingGroupYear(Santa_GiftingGroup dbGiftingGroup)
    {
        Santa_GiftingGroupYear? dbGiftingGroupYear;

        dbGiftingGroupYear = new Santa_GiftingGroupYear
        {
            GiftingGroup = dbGiftingGroup,
            CalendarYear = Item.CalendarYear,
            CurrencyCode = dbGiftingGroup.GetCurrencyCode(),
            CurrencySymbol = dbGiftingGroup.GetCurrencySymbol()
        };

        dbGiftingGroup.Years.Add(dbGiftingGroupYear);
        DbContext.ChangeTracker.DetectChanges();
        return dbGiftingGroupYear;
    }

    protected void AddOrUpdateUserGroupYear(Santa_GiftingGroupYear dbGiftingGroupYear, bool? included,
        int santaUserKey, string name, Santa_User? dbSantaUser = null)
    {
        Santa_YearGroupUser? dbYearUser = dbGiftingGroupYear.Users.FirstOrDefault(x => x.SantaUserKey == santaUserKey);

        if (dbYearUser == null)
        {
            dbSantaUser ??= DbContext.Santa_Users.FirstOrDefault(x => x.SantaUserKey == santaUserKey);

            if (dbSantaUser == null)
            {
                AddGeneralValidationError($"User {name} could not be found.");
            }
            else
            {
                dbYearUser = new Santa_YearGroupUser
                {
                    GiftingGroupYearKey = dbGiftingGroupYear.GiftingGroupYearKey,
                    GiftingGroupYear = dbGiftingGroupYear,
                    SantaUserKey = dbSantaUser.SantaUserKey,
                    SantaUser = dbSantaUser,
                    Included = included
                };

                dbGiftingGroupYear.Users.Add(dbYearUser);
            }
        }
        else
        {
            dbYearUser.Included = included;
        }
    }
}
