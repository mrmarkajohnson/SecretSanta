using Application.Shared.Requests;
using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Commands;

public abstract class GiftingGroupYearBaseCommand<TItem> : BaseCommand<TItem> where TItem : IGiftingGroupYearBase
{
    protected GiftingGroupYearBaseCommand(TItem item) : base(item)
    {
    }

    protected Santa_GiftingGroupYear CreateGiftingGroupYear(Santa_GiftingGroup dbGroup)
    {
        Santa_GiftingGroupYear? dbGiftingGroupYear;

        dbGiftingGroupYear = new Santa_GiftingGroupYear
        {
            GiftingGroup = dbGroup,
            Year = Item.Year,
            CurrencyCode = dbGroup.GetCurrencyCode(),
            CurrencySymbol = dbGroup.GetCurrencySymbol()
        };

        dbGroup.Years.Add(dbGiftingGroupYear);
        DbContext.ChangeTracker.DetectChanges();
        return dbGiftingGroupYear;
    }

    protected void AddOrUpdateUserGroupYear(Santa_GiftingGroupYear dbGiftingGroupYear, bool? included,
        int santaUserId, string name, Santa_User? dbSantaUser = null)
    {
        Santa_YearGroupUser? dbYearUser = dbGiftingGroupYear.Users.FirstOrDefault(x => x.SantaUserId == santaUserId);

        if (dbYearUser == null)
        {
            dbSantaUser ??= DbContext.Santa_Users.FirstOrDefault(x => x.Id == santaUserId);

            if (dbSantaUser == null)
            {
                AddGeneralValidationError($"User {name} could not be found.");
            }
            else
            {
                dbYearUser = new Santa_YearGroupUser
                {
                    YearId = dbGiftingGroupYear.Id,
                    Year = dbGiftingGroupYear,
                    SantaUserId = dbSantaUser.Id,
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
