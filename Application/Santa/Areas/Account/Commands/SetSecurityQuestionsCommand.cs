using Global.Abstractions.Global.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Commands;

public class SetSecurityQuestionsCommand<TItem> : UserBaseCommand<TItem> where TItem : ISetSecurityQuestions
{
    private readonly ClaimsPrincipal _user;

    public SetSecurityQuestionsCommand(TItem item,
        ClaimsPrincipal user,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : base(item, userManager, signInManager)
    {
        _user = user;
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
        EnsureSignedIn(_user, SignInManager);

        string? userId = UserManager.GetUserId(_user);
        if (userId != null)
        {
            var globalUserDb = GetGlobalUser(userId);

            if (globalUserDb != null)
            {
                if (Item.Update)
                {
                    bool passwordCorrect = await CheckPasswordAndHandleFailure(Item, globalUserDb);
                    if (!passwordCorrect || !Validation.IsValid)
                    {
                        return await Result();
                    }
                }
                else if (!Validation.IsValid)
                {
                    return await Result();
                }

                globalUserDb.SecurityQuestion1 = Item.SecurityQuestion1;
                globalUserDb.SecurityAnswer1 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer1?.ToLower() ?? "", globalUserDb);
                globalUserDb.SecurityHint1 = EncryptionHelper.TwoWayEncrypt(Item.SecurityHint1, false);
                globalUserDb.SecurityQuestion2 = Item.SecurityQuestion2;
                globalUserDb.SecurityAnswer2 = EncryptionHelper.OneWayEncrypt(Item.SecurityAnswer2?.ToLower() ?? "", globalUserDb);
                globalUserDb.SecurityHint2 = EncryptionHelper.TwoWayEncrypt(Item.SecurityHint2, false);
                globalUserDb.Greeting = EncryptionHelper.TwoWayEncrypt(Item.Greeting, false, globalUserDb.Id);

                await ModelContext.SaveChangesAsync();
                Success = true;
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