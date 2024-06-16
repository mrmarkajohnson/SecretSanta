using Application.Santa.Areas.Account.Actions;
using Data.Entities.Santa;
using Data.Entities.Shared;
using FluentValidation.Results;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public class CreateSantaUserCommand<TItem> : IdentityBaseCommand<TItem> where TItem : IRegisterSantaUser
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public CreateSantaUserCommand(TItem item,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager) : base(item, userManager, userStore)
    {
        _signInManager = signInManager;
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
        string? originalUserName = Item.UserName;
        string? originalEmail = Item.Email;

        await Send(new HashUserIdentificationAction(Item));

        var globalUserDb = new Global_User
        {
            Forename = Item.Forename,
            MiddleNames = Item.MiddleNames,
            Surname = Item.Surname,
            Email = Item.Email,
            UserName = Item.UserName,
        };

        var santaUserDb = new Santa_User
        {
            GlobalUserId = globalUserDb.Id,
            GlobalUser = globalUserDb
        };

        globalUserDb.SantaUser = santaUserDb;

        ModelContext.ChangeTracker.DetectChanges();

        IdentityResult result = await UserManager.CreateAsync(globalUserDb, Item.Password);

        if (result.Succeeded)
        {
            await SetUserName(globalUserDb);
            await StoreEmailAddress(globalUserDb);

            Item.Password = "";
            await ModelContext.SaveChangesAsync();
            Success = true;
            await _signInManager.SignInAsync(globalUserDb, isPersistent: false);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                string message = error.Description;

                message = ReplaceHashedDetails(message, originalUserName, originalEmail);

                if (message.ToLower().Contains("username"))
                {
                    Validation.Errors.Add(new ValidationFailure(nameof(Item.UserName), message));
                }
                else if (message.ToLower().Contains("email") || message.ToLower().Contains("e-mail"))
                {
                    Validation.Errors.Add(new ValidationFailure(nameof(Item.Email), message));
                }
                else
                {
                    Validation.Errors.Add(new ValidationFailure(nameof(Item.Password), message));
                }
            }
        }

        return await Result();
    }
}
