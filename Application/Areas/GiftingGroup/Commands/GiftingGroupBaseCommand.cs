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

        ClearOpenInvitationsAndApplications(dbGiftingGroup, dbSantaUser);

        AddToCurrentYear(dbGiftingGroup, dbSantaUser);
    }

    private static void ClearOpenInvitationsAndApplications(Santa_GiftingGroup dbGiftingGroup, Santa_User dbSantaUser)
    {
        var dbOpenInvitations = dbGiftingGroup.Invitations
            .Where(x => x.DateArchived == null)
            .Where(x => x.ToSantaUserKey == dbSantaUser.SantaUserKey)
            .ToList();

        dbOpenInvitations.ForEach(x => x.DateArchived = DateTime.Now);

        var dbOpenApplications = dbGiftingGroup.MemberApplications
            .Where(x => x.DateArchived == null)
            .Where(x => x.SantaUserKey == dbSantaUser.SantaUserKey)
            .ToList();

        dbOpenApplications.ForEach(x => x.DateArchived = DateTime.Now);
    }

    private static void AddToCurrentYear(Santa_GiftingGroup dbGiftingGroup, Santa_User dbSantaUser)
    {        
        var dbGiftingGroupYear = dbGiftingGroup.Years.FirstOrDefault(x => x.CalendarYear >= GlobalSettings.CurrentYear 
            && x.Users.Any(x => x.RecipientSantaUserKey != null));

        if (dbGiftingGroupYear != null)
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
