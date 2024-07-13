using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Extensions;
using Global.Abstractions.Global.Account;

namespace Application.Santa.Areas.Account.Queries;

internal class GetUnHashedIdentificationQuery : BaseQuery<UnHashedUserIdWithGreeting>
{
    private readonly IIdentityUser _user;

    public GetUnHashedIdentificationQuery(IIdentityUser user)
    {
        _user = user;
    }

    public override Task<UnHashedUserIdWithGreeting> Handle()
    {
        string? email = string.IsNullOrWhiteSpace(_user.Email) ? null
            : _user.IdentificationHashed ? EncryptionHelper.Decrypt(_user.Email.TrimEnd(IdentitySettings.StandardEmailEnd), true)
            : _user.Email;

        string? userName = string.IsNullOrWhiteSpace(_user.UserName) ? email
            : _user.IdentificationHashed ? EncryptionHelper.Decrypt(_user.UserName, true)
            : _user.UserName;

        string greeting = _user.IdentificationHashed ? EncryptionHelper.Decrypt(_user.Greeting, false, _user.Id) : 
            _user.Greeting;

        return Task.FromResult(new UnHashedUserIdWithGreeting
        {
            Email = email,
            UserName = userName ?? "",
            Greeting = greeting
        });
    }
}
