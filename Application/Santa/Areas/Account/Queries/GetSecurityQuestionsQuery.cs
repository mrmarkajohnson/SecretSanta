using Application.Santa.Global;
using Application.Shared.Identity;
using Data.Entities.Shared;
using Global.Abstractions.Global;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Queries;

public class GetSecurityQuestionsQuery : BaseQuery<ISecurityQuestions?>
{
    private readonly ClaimsPrincipal? _user;
    private readonly string? _userName;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public GetSecurityQuestionsQuery(ClaimsPrincipal user, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // TODO: Add more parameters for a user to reset their login
    public GetSecurityQuestionsQuery(string userName, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userName = userName;
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

        if (securityQuestions == null && !string.IsNullOrEmpty(_userName))
        {
            globalUserDb = ModelContext.Global_Users.FirstOrDefault(x => x.UserName == _userName);
        }

        if (globalUserDb != null)
        {
            securityQuestions = new SecurityQuestions
            {
                SecurityQuestion1 = globalUserDb.SecurityQuestion1,
                SecurityAnswer1 = globalUserDb.SecurityAnswer1,
                SecurityHint1 = globalUserDb.SecurityHint1,
                SecurityQuestion2 = globalUserDb.SecurityQuestion2,
                SecurityAnswer2 = globalUserDb.SecurityAnswer2,
                SecurityHint2 = globalUserDb.SecurityHint2
            };
        }

        return Task.FromResult(securityQuestions);
    }
}
