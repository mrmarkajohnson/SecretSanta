using Application.Santa.Areas.Account.BaseModels;
using Application.Santa.Areas.Account.Queries;
using Global.Abstractions.Global.Account;

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
            UnHashedUserIdWithGreeting unHashedId = await Send(new GetUnHashedIdentificationQuery(_identityUser));

            _identityUser.UserName = unHashedId.UserName;
            _identityUser.Email = unHashedId.Email;
            _identityUser.Greeting = unHashedId.Greeting;
            _identityUser.IdentificationHashed = false;
        }

        return SuccessResult;
    }
}
