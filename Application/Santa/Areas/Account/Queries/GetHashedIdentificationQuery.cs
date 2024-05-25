using Application.Santa.Areas.Account.BaseModels;
using Application.Shared.Identity;
using Global;

namespace Application.Santa.Areas.Account.Queries;

public class GetHashedIdentificationQuery : BaseQuery<HashedUserId>
{
    private readonly IIdentityUser _user;

    public GetHashedIdentificationQuery(IIdentityUser user)
    {
        _user = user;
    }

    public GetHashedIdentificationQuery(string userNameOrEmail, bool hashed)
    {
        _user ??= new CoreIdentityUser
        {
            UserName = userNameOrEmail,
            Email = EmailHelper.IsEmail(userNameOrEmail) ? userNameOrEmail : null,
            IdentificationHashed = hashed
        };
    }

    public override Task<HashedUserId> Handle()
    {
        string? emailHash = string.IsNullOrWhiteSpace(_user.Email) ? null 
            : _user.IdentificationHashed ? _user.Email 
            : EncryptionHelper.TwoWayEncrypt(_user.Email, true) + Settings.StandardEmailEnd; // retain the e-mail format for validation

        string? userNameHash = string.IsNullOrWhiteSpace(_user.UserName) ? emailHash 
            : _user.IdentificationHashed ? _user.UserName
            : EncryptionHelper.TwoWayEncrypt(_user.UserName, true);

        return Task.FromResult(new HashedUserId
        {
            EmailHash = emailHash,
            UserNameHash = userNameHash ?? "",
        });
    }
}
