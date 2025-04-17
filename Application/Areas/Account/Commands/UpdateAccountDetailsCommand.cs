using Application.Areas.Account.Actions;
using Global.Abstractions.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Areas.Account.Commands;

public sealed class UpdateAccountDetailsCommand<TItem> : IdentityBaseCommand<TItem> where TItem : IUpdateSantaUser
{
    public UpdateAccountDetailsCommand(TItem item, IUserStore<IdentityUser> userStore) : base(item, userStore)
    {
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        EnsureSignedIn();

        string? globalUserId = GetCurrentUserId();
        if (globalUserId != null && globalUserId == Item.GlobalUserId)
        {
            Global_User? dbCurrentUser = GetGlobalUser(globalUserId);

            if (dbCurrentUser != null)
            {
                bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, dbCurrentUser);
                if (!passwordCorrect || !Validation.IsValid)
                {
                    return await Result();
                }
                else
                {
                    string? originalUserName = Item.UserName;
                    string? originalEmail = Item.Email;
                    await Send(new HashUserIdentificationAction(Item));

                    if (Item.UserName != dbCurrentUser.UserName)
                    {
                        try
                        {
                            await SetUserName(dbCurrentUser);
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                            message = ReplaceHashedDetails(message, originalUserName, originalEmail);
                            AddValidationError(nameof(Item.UserName), message);
                        }
                    }

                    if (Item.Email != dbCurrentUser.Email)
                    {
                        try
                        {
                            await StoreEmailAddress(dbCurrentUser);
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
                        dbCurrentUser.Forename = Item.Forename;
                        dbCurrentUser.MiddleNames = Item.MiddleNames;
                        dbCurrentUser.Surname = Item.Surname;
                        dbCurrentUser.Email = Item.Email; // just in case
                        dbCurrentUser.UserName = Item.UserName; // just in case

                        return await SaveAndReturnSuccess();
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
