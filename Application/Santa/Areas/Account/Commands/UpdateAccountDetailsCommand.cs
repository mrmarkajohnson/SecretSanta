using Application.Santa.Areas.Account.Actions;
using Global.Abstractions.Santa.Areas.Account;
using Global.Extensions.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Commands;

public class UpdateAccountDetailsCommand<TItem> : IdentityBaseCommand<TItem> where TItem : IUpdateSantaUser
{
    private readonly ClaimsPrincipal _user;

    public UpdateAccountDetailsCommand(TItem item,
        ClaimsPrincipal user,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager) : base(item, userManager, userStore, signInManager)
    {
        _user = user;
        SignInManager = signInManager;
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
        EnsureSignedIn(_user, SignInManager);

        string? userId = UserManager.GetUserId(_user);
        if (userId != null && userId == Item.Id)
        {
            var globalUserDb = GetGlobalUser(userId);

            if (globalUserDb != null)
            {
                bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, globalUserDb);
                if (!passwordCorrect || !Validation.IsValid)
                {
                    return await Result();
                }
                else
                {
                    string? originalUserName = Item.UserName;
                    string? originalEmail = Item.Email;
                    await Send(new HashUserIdentificationAction(Item));

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

        return await Result();
    }
}
