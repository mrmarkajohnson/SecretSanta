using Application.Santa.Areas.Account.Actions;
using Application.Santa.Areas.Account.BaseModels;
using Data.Entities.Shared;
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

    protected override async Task<ISantaUser?> Handle()
    {
        ISantaUser? santaUser = null;

        Global_User? globalUserDb = GetCurrentGlobalUser(_user, _signInManager, _userManager);

        if (globalUserDb != null)
        {
            santaUser = Mapper.Map<SantaUser>(globalUserDb);

            if (_unHashResults)
            {
                await Send(new UnHashUserIdentificationAction(santaUser));
            }
        }

        return santaUser;
    }
}
