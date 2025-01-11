using Application.Areas.Account.BaseModels;
using Application.Areas.Account.Queries;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Actions;

internal class HashUserIdentificationAction : BaseAction<IIdentityUser>
{
    private readonly IIdentityUser _identityUser;

    public HashUserIdentificationAction(IIdentityUser identityUser)
    {
        _identityUser = identityUser;
    }

    protected async override Task<bool> Handle()
    {
        if (!_identityUser.IdentificationHashed)
        {
            HashedUserIdWithGreeting hashedId = await Send(new GetHashedIdWithGreetingQuery(_identityUser));

            _identityUser.UserName = hashedId.UserNameHash;
            _identityUser.Email = hashedId.EmailHash;
            _identityUser.Greeting = hashedId.GreetingHash;
            _identityUser.IdentificationHashed = true;
        }

        return SuccessResult;
    }
}
