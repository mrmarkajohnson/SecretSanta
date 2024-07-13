using Global.Abstractions.Global.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Commands;

public class SetSecurityQuestionsCommand<TItem> : BaseCommand<TItem> where TItem : ISecurityQuestions
{
    private readonly ClaimsPrincipal _user;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public SetSecurityQuestionsCommand(TItem item,
        ClaimsPrincipal user,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : base(item)
    {
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    protected override async Task<ICommandResult<TItem>> HandlePostValidation()
    {
        if (_signInManager.IsSignedIn(_user))
        {
            string? userId = _userManager.GetUserId(_user);
            if (userId != null)
            {
                var globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.Id == userId);

                if (globalUserDb != null)
                {
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
        }
        else
        {
            AddUserNotFoundError();
        }

        return await Result();
    }
}
