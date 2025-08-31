using Application.Areas.Account.BaseModels;
using Global.Abstractions.Areas.Account;

namespace Application.Shared.Helpers;

public static class UserEncryptionHelper
{
    public static TItem UnHash<TItem>(this TItem hashableUser) where TItem : IHashableUser
    {
        if (hashableUser.IdentificationHashed)
        {
            UnHashedUserWithGreeting unHashedId = hashableUser.GetUnhashedDetails();

            hashableUser.UserName = unHashedId.UserName;
            hashableUser.Email = unHashedId.Email;

            if (hashableUser is IIdentityUser identityUser)
            {
                identityUser.Greeting = unHashedId.Greeting;
            }

            hashableUser.IdentificationHashed = false;
        }

        return hashableUser; // allows this to be used in selects etc.
    }

    public static UnHashedUserWithGreeting GetUnhashedDetails(this IHashableUser hashableUser)
    {
        string? email = string.IsNullOrWhiteSpace(hashableUser.Email)
            ? null
            : hashableUser.IdentificationHashed
                ? EncryptionHelper.Decrypt(hashableUser.Email.TrimEnd(IdentitySettings.StandardEmailEnd), true)
                : hashableUser.Email;

        string? userName = string.IsNullOrWhiteSpace(hashableUser.UserName)
            ? email
            : hashableUser.IdentificationHashed
                ? EncryptionHelper.Decrypt(hashableUser.UserName, true)
                : hashableUser.UserName;

        string? greeting = null;

        if (hashableUser is IIdentityUser identityUser)
        {
            greeting = identityUser.IdentificationHashed
                ? EncryptionHelper.Decrypt(identityUser.Greeting, false, hashableUser.GlobalUserId)
                : identityUser.Greeting;
        }

        return new UnHashedUserWithGreeting
        {
            Email = email,
            UserName = userName ?? string.Empty,
            Greeting = greeting ?? string.Empty
        };
    }
}
