using Application.Santa.Areas.Account.BaseModels;
using Application.Santa.Areas.Account.Queries;

namespace Application.Santa.Areas.Account.Actions;

internal class HashUserIdentificationAction : BaseAction<IIdentityUser>
{
    private readonly IIdentityUser _identityUser;

    public HashUserIdentificationAction(IIdentityUser identityUser)
    {
        _identityUser = identityUser;
    }

    public async override Task<bool> Handle()
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
