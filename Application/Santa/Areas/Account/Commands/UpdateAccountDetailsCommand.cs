using Application.Santa.Areas.Account.Actions;
using FluentValidation.Results;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Commands;

public class UpdateAccountDetailsCommand<TItem> : IdentityBaseCommand<TItem> where TItem : IUpdateSantaUser
{
    private readonly ClaimsPrincipal _user;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UpdateAccountDetailsCommand(TItem item,
        ClaimsPrincipal user,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager) : base(item, userManager, userStore)
    {
        _user = user;
        _signInManager = signInManager;
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
        if (_signInManager.IsSignedIn(_user))
        {
            string? userId = UserManager.GetUserId(_user);
            if (userId != null && userId == Item.Id)
            {
                var globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.Id == userId);

                if (globalUserDb != null)
                {
                    string? originalUserName = Item.UserName;
                    string? originalEmail = Item.Email;

                    await Send(new HashUserIdentificationAction(Item));

                    bool passwordCorrect = await UserManager.CheckPasswordAsync(globalUserDb, Item.CurrentPassword);
                    if (!passwordCorrect)
                    {
                        AddValidationError(nameof(Item.CurrentPassword), "Current Password is incorrect.");
                    }
                    else if (Validation.IsValid)
                    {
                        if (Item.UserName != globalUserDb.UserName)
                        {
                            try
                            {
                                await SetUserName(globalUserDb);
                            }
                            catch (Exception ex)
                            {
                                string message = ex.Message;
                                message = ReplaceHashedDetails(message, originalUserName, originalEmail);
                                AddValidationError(nameof(Item.UserName), message);
                            }
                        }

                        if (Item.Email != globalUserDb.Email)
                        {
                            try
                            {
                                await StoreEmailAddress(globalUserDb);
                            }
                            catch (Exception ex)
                            {
                                string message = ex.Message;
                                message = ReplaceHashedDetails(message, originalUserName, originalEmail);
                                AddValidationError(nameof(Item.Email), message);
                            }
                        }

                        if (Validation.IsValid)
                        {
                            globalUserDb.Forename = Item.Forename;
                            globalUserDb.MiddleNames = Item.MiddleNames;
                            globalUserDb.Surname = Item.Surname;
                            globalUserDb.Email = Item.Email; // just in case
                            globalUserDb.UserName = Item.UserName; // just in case

                            await ModelContext.SaveChangesAsync();
                            Success = true;
                        }
                    }
                }
                else
                {
                    AddUserNotFoundError();
                }
            }
            else
            {
                AddUserNotFoundError();
            }
        }

        return await Result();
    }
}
