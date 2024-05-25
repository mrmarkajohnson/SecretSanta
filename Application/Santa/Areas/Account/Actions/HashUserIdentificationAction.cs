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
            HashedUserId hashedId = await Send(new GetHashedIdentificationQuery(_identityUser));

            _identityUser.UserName = hashedId.UserNameHash;
            _identityUser.Email = hashedId.EmailHash;
            _identityUser.IdentificationHashed = true;
        }

        return SuccessResult;
    }
}
