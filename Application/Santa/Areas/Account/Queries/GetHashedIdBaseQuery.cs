using Application.Santa.Areas.Account.BaseModels;
using Global.Abstractions.Global.Account;

namespace Application.Santa.Areas.Account.Queries;

internal class GetHashedIdBaseQuery<T> : BaseQuery<T> where T : HashedUserId, new()
{
    protected IIdentityUser User { get; set; }

    public GetHashedIdBaseQuery(IIdentityUser user)
    {
        User = user;
    }

    public override Task<T> Handle()
    {
        string? emailHash = string.IsNullOrWhiteSpace(User.Email) ? null
            : User.IdentificationHashed ? User.Email
            : EncryptionHelper.TwoWayEncrypt(User.Email, true) + IdentitySettings.StandardEmailEnd; // retain the e-mail format for validation

        string? userNameHash = string.IsNullOrWhiteSpace(User.UserName) ? emailHash
            : User.IdentificationHashed ? User.UserName
            : EncryptionHelper.TwoWayEncrypt(User.UserName, true);

        var result = Task.FromResult(new T
        {
            EmailHash = emailHash,
            UserNameHash = userNameHash ?? "",
        });

        if (result is HashedUserIdWithGreeting greetingResult)
        {
            greetingResult.GreetingHash = User.IdentificationHashed ? User.Greeting
                : EncryptionHelper.TwoWayEncrypt(User.Greeting, false, User.Id);
        }

        return result;
    }
}
