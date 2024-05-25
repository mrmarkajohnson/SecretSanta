using Application.Santa.Areas.Account.BaseModels;
using Application.Shared.Identity;
using Global;

namespace Application.Santa.Areas.Account.Queries;

public class GetUnHashedIdentificationQuery : BaseQuery<UnHashedUserId>
{
    private readonly IIdentityUser _user;

    public GetUnHashedIdentificationQuery(IIdentityUser user)
    {
        _user = user;
    }

    public GetUnHashedIdentificationQuery(string userNameOrEmail, bool hashed)
    {
        _user ??= new CoreIdentityUser
        {
            UserName = userNameOrEmail,
            Email = EmailHelper.IsEmail(userNameOrEmail) ? userNameOrEmail : null,
            IdentificationHashed = hashed
        };
    }

    public override Task<UnHashedUserId> Handle()
    {
        string? email = string.IsNullOrWhiteSpace(_user.Email) ? null
            : _user.IdentificationHashed ? EncryptionHelper.Decrypt(_user.Email.TrimEnd(Settings.StandardEmailEnd), true)
            : _user.Email;

        string? userName = string.IsNullOrWhiteSpace(_user.UserName) ? email
            : _user.IdentificationHashed ? EncryptionHelper.Decrypt(_user.UserName, true)
            : _user.UserName;

        return Task.FromResult(new UnHashedUserId
        {
            Email = email,
            UserName = userName ?? "",
        });
    }
}
