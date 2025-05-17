using Application.Areas.Account.Actions;
using Global.Abstractions.Areas.Account;
using Global.Settings;
using Microsoft.AspNetCore.Identity;

namespace Application.Areas.Account.Commands;

public sealed class CreateSantaUserCommand<TItem> : IdentityBaseCommand<TItem> where TItem : IRegisterSantaUser
{
    public CreateSantaUserCommand(TItem item, IUserStore<IdentityUser> userStore) : base(item, userStore)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        string? originalUserName = Item.UserName;
        string? originalEmail = Item.Email;

        Item.Greeting = Greetings.Messages.Get1FromList();

        await Send(new HashUserIdentificationAction(Item));

        var dbGlobalUser = new Global_User
        {
            Forename = Item.Forename.Trim(),            
            Surname = Item.Surname.Trim(),
            Email = Item.Email.NullIfEmpty(),
            UserName = Item.UserName.NullIfEmpty(),
            Greeting = Item.Greeting
        };

        SetOtherNames(dbGlobalUser);

        var dbSantaUser = new Santa_User
        {
            GlobalUserId = dbGlobalUser.Id,
            GlobalUser = dbGlobalUser
        };

        dbGlobalUser.SantaUser = dbSantaUser;

        DbContext.ChangeTracker.DetectChanges();

        IdentityResult result = await UserManager.CreateAsync(dbGlobalUser, Item.Password);

        if (result.Succeeded)
        {
            await SetUserName(dbGlobalUser);
            await StoreEmailAddress(dbGlobalUser);

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
}
