using Application.Santa.Areas.Account.BaseModels;
using Application.Santa.Global;
using Global.Abstractions.Santa.Areas.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.Account.Queries;

public class GetCurrentUserQuery : BaseQuery<ISantaUser?>
{
    private readonly ClaimsPrincipal _user;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public GetCurrentUserQuery(ClaimsPrincipal user, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public override Task<ISantaUser?> Handle()
    {
        ISantaUser? santaUser = null;

        if (_signInManager.IsSignedIn(_user))
        {
            string? userId = _userManager.GetUserId(_user);
            if (userId != null)
            {
                var globalUserDb = ModelContext.Global_Users
                    .FirstOrDefault(x => x.Id == userId);

                if (globalUserDb != null)
                {
                    santaUser = new SantaUser
                    {
                        Id = globalUserDb.Id,
                        UserName = globalUserDb.UserName,
                        Email = globalUserDb.Email,
                        Forename = globalUserDb.Forename,
                        MiddleNames = globalUserDb.MiddleNames,
                        Surname = globalUserDb.Surname,
                        SecurityQuestionsSet = globalUserDb.SecurityQuestionsSet
                    };
                }
            }
        }

        return Task.FromResult(santaUser);
    }
}
