using Application.Santa.Areas.Account.Actions;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;

namespace Application.Santa.Areas.Account.Commands;

public class UpdateAccountDetailsCommand<TItem> : IdentityBaseCommand<TItem> where TItem : IUpdateSantaUser
{
    public UpdateAccountDetailsCommand(TItem item, IUserStore<IdentityUser> userStore) : base(item, userStore)
    {
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
        EnsureSignedIn();

        string? userId = GetCurrentUserId();
        if (userId != null && userId == Item.Id)
        {
            Global_User? dbGlobalUser = GetGlobalUser(userId);

            if (dbGlobalUser != null)
            {
                bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, dbGlobalUser);
                if (!passwordCorrect || !Validation.IsValid)
                {
                    return await Result();
                }
                else
                {
                    string? originalUserName = Item.UserName;
                    string? originalEmail = Item.Email;
                    await Send(new HashUserIdentificationAction(Item));

                    if (Item.UserName != dbGlobalUser.UserName)
                    {
                        try
                        {
                            await SetUserName(dbGlobalUser);
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                            message = ReplaceHashedDetails(message, originalUserName, originalEmail);
                            AddValidationError(nameof(Item.UserName), message);
                        }
                    }

                    if (Item.Email != dbGlobalUser.Email)
                    {
                        try
                        {
                            await StoreEmailAddress(dbGlobalUser);
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
                        dbGlobalUser.Forename = Item.Forename;
                        dbGlobalUser.MiddleNames = Item.MiddleNames;
                        dbGlobalUser.Surname = Item.Surname;
                        dbGlobalUser.Email = Item.Email; // just in case
                        dbGlobalUser.UserName = Item.UserName; // just in case

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
