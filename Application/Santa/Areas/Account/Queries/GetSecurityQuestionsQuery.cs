using Application.Shared.Identity;
using Data.Entities.Shared;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Queries;

public class GetSecurityQuestionsQuery : BaseQuery<ISecurityQuestions?>
{
    private readonly ClaimsPrincipal? _user;
    private readonly string? _hashedUserName;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public GetSecurityQuestionsQuery(ClaimsPrincipal user, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public GetSecurityQuestionsQuery(string userName, bool userNameHashed, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _hashedUserName = userNameHashed ? userName : EncryptionHelper.TwoWayEncrypt(userName, true);
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public override Task<ISecurityQuestions?> Handle()
    {
        ISecurityQuestions? securityQuestions = null;
        Global_User? globalUserDb = null;

        if (_user != null && _signInManager.IsSignedIn(_user))
        {
            string? userId = _userManager.GetUserId(_user);
            if (userId != null)
            {
                globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.Id == userId);
            }
        }

        if (globalUserDb == null && !string.IsNullOrEmpty(_hashedUserName))
        {
            globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.UserName == _hashedUserName);
        }

        if (globalUserDb != null)
        {
            securityQuestions = new SecurityQuestions
            {
                SecurityQuestion1 = globalUserDb.SecurityQuestion1,
                SecurityAnswer1 = globalUserDb.SecurityAnswer1,
                SecurityHint1 = EncryptionHelper.Decrypt(globalUserDb.SecurityHint1, false),
                SecurityQuestion2 = globalUserDb.SecurityQuestion2,
                SecurityAnswer2 = globalUserDb.SecurityAnswer2,
                SecurityHint2 = EncryptionHelper.Decrypt(globalUserDb.SecurityHint2, false),
                Greeting = "" // EncryptionHelper.Decrypt(globalUserDb.Greeting, false)
            };
        }

        return Task.FromResult(securityQuestions);
    }
}
