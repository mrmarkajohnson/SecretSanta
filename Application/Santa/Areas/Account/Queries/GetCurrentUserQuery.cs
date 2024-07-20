using Application.Santa.Areas.Account.Actions;
using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Queries;

public class GetCurrentUserQuery : BaseQuery<ISantaUser?>
{
    private readonly ClaimsPrincipal _user;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private bool _unHashResults;

    public GetCurrentUserQuery(ClaimsPrincipal user, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, bool unHashResults)
    {
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
        _unHashResults = unHashResults;
    }

    public override async Task<ISantaUser?> Handle()
    {
        ISantaUser? santaUser = null;

        if (_signInManager.IsSignedIn(_user))
        {
            string? userId = _userManager.GetUserId(_user);
            if (userId != null)
            {
                var globalUserDb = GetGlobalUser(userId);

                if (globalUserDb != null)
                {
                    santaUser = new SantaUser
                    {
                        Id = globalUserDb.Id,
                        UserName = globalUserDb.UserName, // note this will be hashed
                        Email = globalUserDb.Email, // note this will be hashed
                        Forename = globalUserDb.Forename,
                        MiddleNames = globalUserDb.MiddleNames,
                        Surname = globalUserDb.Surname,
                        Greeting = globalUserDb.Greeting,
                        SecurityQuestionsSet = globalUserDb.SecurityQuestionsSet,
                        IdentificationHashed = true
                    };

                    if (_unHashResults)
                    {
                        await Send(new UnHashUserIdentificationAction(santaUser));
                    }
                }
            }
        }

        return santaUser;
    }
}
