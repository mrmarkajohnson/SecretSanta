using Application.Areas.Account.BaseModels;
using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Queries;

internal class GetHashedIdBaseQuery<T> : BaseQuery<T> where T : HashedUser, new()
{
    protected IIdentityUser IdentityUser { get; set; }

    public GetHashedIdBaseQuery(IIdentityUser user)
    {
        IdentityUser = user;
    }

    protected override Task<T> Handle()
    {
        string? emailHash = string.IsNullOrWhiteSpace(IdentityUser.Email) ? null
            : IdentityUser.IdentificationHashed ? IdentityUser.Email
            : EncryptionHelper.EncryptEmail(IdentityUser.Email);

        string? userNameHash = string.IsNullOrWhiteSpace(IdentityUser.UserName) ? emailHash
            : IdentityUser.IdentificationHashed ? IdentityUser.UserName
            : EncryptionHelper.TwoWayEncrypt(IdentityUser.UserName, true);

        var result = Task.FromResult(new T
        {
            EmailHash = emailHash,
            UserNameHash = userNameHash ?? string.Empty,
        });

        if (result is HashedUserWithGreeting greetingResult)
        {
            greetingResult.GreetingHash = IdentityUser.IdentificationHashed ? IdentityUser.Greeting
                : EncryptionHelper.TwoWayEncrypt(IdentityUser.Greeting, false, IdentityUser.GlobalUserId);
        }

        return result;
    }
}
