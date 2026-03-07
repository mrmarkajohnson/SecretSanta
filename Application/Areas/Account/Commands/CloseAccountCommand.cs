namespace Application.Areas.Account.Commands;

public class CloseAccountCommand : BaseCommand<string>
{
    public CloseAccountCommand(string? message) : base(message ?? "")
    {
    }

    protected async override Task<ICommandResult<string>> HandlePostValidation()
    {
        if (Item != IdentitySettings.PleaseCloseAccount)
        {
            AddGeneralValidationError("Please enter the exact phrase shown on screen, to confirm you want to close your account.");
            return await Result();
        }

        Global_User dbCurrentUser = GetCurrentGlobalUser();

        if (dbCurrentUser.SystemAdmin)
        {
            AddGeneralValidationError("System administrators cannot close their account. Please make another user a system administrator first.");
            return await Result();
        }

        DateTime now = DateTime.Now; // allows tracking of the deletion

        dbCurrentUser.Email = null;
        dbCurrentUser.Forename = "Previous";
        dbCurrentUser.MiddleNames = null;
        dbCurrentUser.Surname = "User";
        dbCurrentUser.PhoneNumber = null; // just in case
        dbCurrentUser.LockoutEnd = DateTime.MaxValue;
        dbCurrentUser.UserName = EncryptionHelper.OneWayEncrypt("PreviousUser", dbCurrentUser, false);
        dbCurrentUser.PreferredNameType = IdentitySettings.PreferredNameOption.Forename;
        dbCurrentUser.PreferredFirstName = null;
        dbCurrentUser.AuditTrail.Clear();

        ClearSecurityQuestions(dbCurrentUser);
        ArchiveGroupLinks(dbCurrentUser, now);

        return await SaveAndReturnSuccess();
    }

    private static void ClearSecurityQuestions(Global_User dbCurrentUser)
    {
        dbCurrentUser.SecurityQuestion1 = null;
        dbCurrentUser.SecurityHint1 = null;
        dbCurrentUser.SecurityAnswer1 = null;
        dbCurrentUser.SecurityQuestion2 = null;
        dbCurrentUser.SecurityHint2 = null;
        dbCurrentUser.SecurityAnswer2 = null;
    }

    private static void ArchiveGroupLinks(Global_User dbCurrentUser, DateTime now)
    {
        if (dbCurrentUser.SantaUser != null)
        {
            foreach (var dbGroupLink in dbCurrentUser.SantaUser.GiftingGroupLinks.ToList())
            {
                dbGroupLink.DateArchived = now;

                var dbActiveYears = dbGroupLink.GiftingGroup.Years
                    .Where(x => x.CalendarYear >= GlobalSettings.CurrentYear)
                    .ToList();

                foreach (var dbYear in dbActiveYears)
                {
                    var dbArchivableUsers = dbYear.Users
                        .Where(x => x.SantaUserKey == dbCurrentUser.SantaUser.SantaUserKey)
                        .Where(x => x.RecipientSantaUser == null)
                        .ToList();

                    foreach (var dbUser in dbArchivableUsers)
                    {
                        dbUser.Included = false;
                    }
                }
            }
        }
    }
}
