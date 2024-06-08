using Application.Santa.Areas.Account.Actions;
using Data.Entities.Santa;
using Data.Entities.Shared;
using FluentValidation.Results;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public class CreateSantaUserCommand<TItem> : BaseCommand<TItem> where TItem : IRegisterSantaUser
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserEmailStore<IdentityUser> _emailStore;

    public CreateSantaUserCommand(TItem item,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager) : base(item)
    {
        _userManager = userManager;
        _userStore = userStore;
        _signInManager = signInManager;
        _emailStore = GetEmailStore();
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

        IdentityResult result = await _userManager.CreateAsync(globalUserDb, Item.Password);

        if (result.Succeeded)
        {
            await _userStore.SetUserNameAsync(globalUserDb, Item.UserName, CancellationToken.None);

            if (!string.IsNullOrWhiteSpace(Item.Email))
            {
                await _emailStore.SetEmailAsync(globalUserDb, Item.Email, CancellationToken.None);
            }

            Success = true;
            Item.Password = "";
            await ModelContext.SaveChangesAsync();
            await _signInManager.SignInAsync(globalUserDb, isPersistent: false);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                string description = error.Description;
                
                if (!string.IsNullOrWhiteSpace(Item.UserName))
                {
                    description = description.Replace(Item.UserName, originalUserName);
                }

                if (!string.IsNullOrWhiteSpace(Item.Email))
                {
                    description = description.Replace(Item.Email, originalEmail);
                }
                
                if (error.Description.ToLower().Contains("username"))
                {
                    Validation.Errors.Add(new ValidationFailure(nameof(Item.UserName), description));
                }
                else
                {
                    Validation.Errors.Add(new ValidationFailure(nameof(Item.Password), description));
                }
            }
        }

        return await Result();
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }

        return (IUserEmailStore<IdentityUser>)_userStore;
    }
}
