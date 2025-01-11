using Application.Areas.Account.BaseModels;
using Application.Shared.Requests;
using Global.Abstractions.Areas.Account;
using Global.Extensions.System;

namespace Application.Areas.Account.Queries;

internal class GetUnHashedIdentificationQuery : BaseQuery<UnHashedUserIdWithGreeting>
{
    private readonly IHashableUserId _hashableUser;

    public GetUnHashedIdentificationQuery(IHashableUserId hashableUser)
    {
        _hashableUser = hashableUser;
    }

    protected override Task<UnHashedUserIdWithGreeting> Handle()
    {
        string? email = string.IsNullOrWhiteSpace(_hashableUser.Email) ? null
            : _hashableUser.IdentificationHashed ? EncryptionHelper.Decrypt(_hashableUser.Email.TrimEnd(IdentitySettings.StandardEmailEnd), true)
            : _hashableUser.Email;

        string? userName = string.IsNullOrWhiteSpace(_hashableUser.UserName) ? email
            : _hashableUser.IdentificationHashed ? EncryptionHelper.Decrypt(_hashableUser.UserName, true)
            : _hashableUser.UserName;

        string? greeting = null;

        if (_hashableUser is IIdentityUser identityUser)
        {
            greeting = identityUser.IdentificationHashed ? EncryptionHelper.Decrypt(identityUser.Greeting, false, _hashableUser.Id) :
            identityUser.Greeting;
        }

        return Task.FromResult(new UnHashedUserIdWithGreeting
        {
            Email = email,
            UserName = userName ?? string.Empty,
            Greeting = greeting ?? string.Empty
        });
    }
}
