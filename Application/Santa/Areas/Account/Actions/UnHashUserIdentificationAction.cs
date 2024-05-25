using Application.Santa.Areas.Account.BaseModels;
using Application.Santa.Areas.Account.Queries;

namespace Application.Santa.Areas.Account.Actions;

internal class UnHashUserIdentificationAction : BaseAction<IIdentityUser>
{
    private readonly IIdentityUser _identityUser;

    public UnHashUserIdentificationAction(IIdentityUser identityUser)
    {
        _identityUser = identityUser;
    }

    public async override Task<bool> Handle()
    {
        if (_identityUser.IdentificationHashed)
        {
            UnHashedUserId unHashedId = await Send(new GetUnHashedIdentificationQuery(_identityUser));

            _identityUser.UserName = unHashedId.UserName;
            _identityUser.Email = unHashedId.Email;
            _identityUser.IdentificationHashed = false;
        }

        return SuccessResult;
    }
}
