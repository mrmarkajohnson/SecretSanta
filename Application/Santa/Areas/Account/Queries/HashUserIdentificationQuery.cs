using Application.Santa.Areas.Account.BaseModels;

namespace Application.Santa.Areas.Account.Queries;

internal class HashUserIdentificationQuery : BaseQuery<IIdentityUser>
{
    private readonly IIdentityUser _identityUser;

    public HashUserIdentificationQuery(IIdentityUser identityUser)
    {
        _identityUser = identityUser;
    }

    public async override Task<IIdentityUser> Handle()
    {
        if (!_identityUser.IdentificationHashed)
        {
            HashedUserId hashedId = await Send(new GetHashedIdentificationQuery(_identityUser));

            _identityUser.UserName = hashedId.UserNameHash;
            _identityUser.Email = hashedId.EmailHash;
            _identityUser.IdentificationHashed = true;
        }

        return _identityUser;
    }
}
