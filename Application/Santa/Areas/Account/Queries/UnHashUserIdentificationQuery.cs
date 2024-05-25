using Application.Santa.Areas.Account.BaseModels;

namespace Application.Santa.Areas.Account.Queries;

internal class UnHashUserIdentificationQuery : BaseQuery<IIdentityUser>
{
    private readonly IIdentityUser _identityUser;

    public UnHashUserIdentificationQuery(IIdentityUser identityUser)
    {
        _identityUser = identityUser;
    }

    public async override Task<IIdentityUser> Handle()
    {
        if (_identityUser.IdentificationHashed)
        {
            UnHashedUserId unHashedId = await Send(new GetUnHashedIdentificationQuery(_identityUser));

            _identityUser.UserName = unHashedId.UserName;
            _identityUser.Email = unHashedId.Email;
            _identityUser.IdentificationHashed = false;
        }

        return _identityUser;
    }
}
