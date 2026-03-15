using Application.Areas.Account.Actions;
using Global.Abstractions.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Areas.Account.Commands;

public sealed class CreateSantaUserCommand<TItem> : IdentityBaseCommand<TItem> where TItem : IRegisterSantaUser
{
    public CreateSantaUserCommand(TItem item, IUserStore<IdentityUser> userStore) : base(item, userStore)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        string? originalUserName = Item.UserName; // get this before it is hashed
        string? originalEmail = Item.Email = TidyEmail(Item.Email); // ditto

        Item.Greeting = Greetings.Messages.Get1FromList();

        await Send(new HashUserIdentificationAction(Item));

        var dbGlobalUser = new Global_User
        {
            Forename = Item.Forename.Trim(),            
            Surname = Item.Surname.Trim(),
            Gender = Item.Gender,
            Email = Item.Email.NullIfEmpty(),
            UserName = Item.UserName.NullIfEmpty(),
            Greeting = Item.Greeting,
            SystemAdmin = DbContext.Global_Users.Count() == 0 // if the first user, make them a system administrator
        };

        SetOtherNames(dbGlobalUser);

        var dbSantaUser = new Santa_User
        {
            GlobalUserId = dbGlobalUser.Id,
            GlobalUser = dbGlobalUser
        };

        dbGlobalUser.SantaUser = dbSantaUser;

        DbContext.ChangeTracker.DetectChanges();

        IdentityResult result = await UserManager.CreateAsync(dbGlobalUser, Item.Password.Trim());

        if (result.Succeeded)
        {
            await SetUserName(dbGlobalUser);
            await StoreEmailAddress(dbGlobalUser, originalEmail);

            HandleOpenInvitations(dbGlobalUser, dbSantaUser);

            Item.Password = string.Empty;
            await DbContext.SaveChangesAsync();
            Success = true;
            await SignInManager.SignInAsync(dbGlobalUser, isPersistent: false);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                string message = error.Description;

                message = ReplaceHashedDetails(message, originalUserName, originalEmail);

                if (message.ToLower().Contains("username"))
                {
                    AddValidationError(nameof(Item.UserName), message);
                }
                else if (message.ToLower().Contains("email") || message.ToLower().Contains("e-mail"))
                {
                    AddValidationError(nameof(Item.Email), message);
                }
                else
                {
                    AddValidationError(nameof(Item.Password), message);
                }
            }
        }

        return await Result();
    }

    private void HandleOpenInvitations(Global_User dbGlobalUser, Santa_User dbSantaUser)
    {
        if (dbGlobalUser.Email != null)
        {
            try
            {
                var dbOpenInvitations = DbContext.Santa_Invitations
                    .Where(GroupInvitationExpressions.IsActive(true))
                    .Where(x => x.ToSantaUserKey == null)
                    .Where(x => x.ToEmailAddress != null && x.ToEmailAddress == dbGlobalUser.Email) // both e-mail addresses are hashed
                    .Where(x => x.ToName != null && (x.ToName.Trim().ToLower() == dbGlobalUser.Forename.Trim().ToLower()
                        || (dbGlobalUser.PreferredFirstName != null && x.ToName.Trim().ToLower() == dbGlobalUser.PreferredFirstName.Trim().ToLower())))
                    .ToList();

                foreach (Santa_Invitation dbInvitation in dbOpenInvitations)
                {
                    dbInvitation.ToSantaUser = dbSantaUser;
                }
            }
            catch { }
        }
    }
}
