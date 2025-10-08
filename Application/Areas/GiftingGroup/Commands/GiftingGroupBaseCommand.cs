namespace Application.Areas.GiftingGroup.Commands;

public abstract class GiftingGroupBaseCommand<TItem> : BaseCommand<TItem>
{
    protected GiftingGroupBaseCommand(TItem item) : base(item)
    {
    }

    protected static void AddToGiftingGroup(Santa_GiftingGroup dbGiftingGroup, Santa_User dbSantaUser)
    {
        dbGiftingGroup.Members.Add(new Santa_GiftingGroupUser
        {
            GiftingGroup = dbGiftingGroup,
            GiftingGroupKey = dbGiftingGroup.GiftingGroupKey,
            SantaUser = dbSantaUser,
            SantaUserKey = dbSantaUser.SantaUserKey,
        });

        AddToCurrentYear(dbGiftingGroup, dbSantaUser);
    }

    private static void AddToCurrentYear(Santa_GiftingGroup dbGiftingGroup, Santa_User dbSantaUser)
    {        
        var dbGiftingGroupYear = dbGiftingGroup.Years.FirstOrDefault(x => x.CalendarYear == DateTime.Today.Year);
        bool alreadyCalculated = dbGiftingGroupYear?.Calculated() ?? false;

        if (!alreadyCalculated)
        {
            if (dbGiftingGroupYear != null)
            {
                alreadyCalculated = dbGiftingGroupYear.Users.Any(x => x.RecipientSantaUserKey != null);
            }
        }

        if (!alreadyCalculated && dbGiftingGroupYear != null)
        {
            dbGiftingGroupYear.Users.Add(new Santa_YearGroupUser
            {
                GiftingGroupYearKey = dbGiftingGroupYear.CalendarYear,
                GiftingGroupYear = dbGiftingGroupYear,
                SantaUserKey = dbSantaUser.SantaUserKey,
                SantaUser = dbSantaUser,
                Included = true
            });
        }
    }
}
