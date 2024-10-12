using Application.Santa.Areas.Account.BaseModels;
using Application.Santa.Areas.Account.Queries;
using Global.Abstractions.Global.Account;
using Global.Abstractions.Global.Shared;

namespace Application.Santa.Areas.Account.Actions;

internal class UnHashUserIdentificationAction : BaseAction<IHashableUserId>
{
    private readonly IHashableUserId _hashableUser;

    public UnHashUserIdentificationAction(IHashableUserId hashableUser)
    {
        _hashableUser = hashableUser;
    }

    protected async override Task<bool> Handle()
    {
        if (_hashableUser.IdentificationHashed)
        {
            UnHashedUserIdWithGreeting unHashedId = await Send(new GetUnHashedIdentificationQuery(_hashableUser));

            _hashableUser.UserName = unHashedId.UserName;
            _hashableUser.Email = unHashedId.Email;

            if (_hashableUser is IIdentityUser identityUser)
            {
                identityUser.Greeting = unHashedId.Greeting;
            }

            _hashableUser.IdentificationHashed = false;
        }

        return SuccessResult;
    }
}
