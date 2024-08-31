using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Global.Account;
using Global.Extensions.System;

namespace Application.Santa.Areas.Account.Queries;

internal class GetUnHashedIdentificationQuery : BaseQuery<UnHashedUserIdWithGreeting>
{
    private readonly IIdentityUser _identityUser;

    public GetUnHashedIdentificationQuery(IIdentityUser identityUser)
    {
        _identityUser = identityUser;
    }

    protected override Task<UnHashedUserIdWithGreeting> Handle()
    {
        string? email = string.IsNullOrWhiteSpace(_identityUser.Email) ? null
            : _identityUser.IdentificationHashed ? EncryptionHelper.Decrypt(_identityUser.Email.TrimEnd(IdentitySettings.StandardEmailEnd), true)
            : _identityUser.Email;

        string? userName = string.IsNullOrWhiteSpace(_identityUser.UserName) ? email
            : _identityUser.IdentificationHashed ? EncryptionHelper.Decrypt(_identityUser.UserName, true)
            : _identityUser.UserName;

        string greeting = _identityUser.IdentificationHashed ? EncryptionHelper.Decrypt(_identityUser.Greeting, false, _identityUser.Id) : 
            _identityUser.Greeting;

        return Task.FromResult(new UnHashedUserIdWithGreeting
        {
            Email = email,
            UserName = userName ?? "",
            Greeting = greeting
        });
    }
}
