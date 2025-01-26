using Application.Areas.Account.BaseModels;
using Global.Abstractions.Areas.Account;
using Global.Extensions.System;

namespace Application.Shared.Helpers;

internal static class UserEncryptionHelper
{
    public static TItem UnHash<TItem>(this TItem hashableUser) where TItem : IHashableUserId
    {
        if (hashableUser.IdentificationHashed)
        {
            UnHashedUserIdWithGreeting unHashedId = hashableUser.GetUnhashedDetails();

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

    public static UnHashedUserIdWithGreeting GetUnhashedDetails(this IHashableUserId hashableUser)
    {
        string? email = string.IsNullOrWhiteSpace(hashableUser.Email) ? null
                    : hashableUser.IdentificationHashed ? EncryptionHelper.Decrypt(hashableUser.Email.TrimEnd(IdentitySettings.StandardEmailEnd), true)
                    : hashableUser.Email;

        string? userName = string.IsNullOrWhiteSpace(hashableUser.UserName) ? email
            : hashableUser.IdentificationHashed ? EncryptionHelper.Decrypt(hashableUser.UserName, true)
            : hashableUser.UserName;

        string? greeting = null;

        if (hashableUser is IIdentityUser identityUser)
        {
            greeting = identityUser.IdentificationHashed 
                ? EncryptionHelper.Decrypt(identityUser.Greeting, false, hashableUser.Id) 
                : identityUser.Greeting;
        }

        return new UnHashedUserIdWithGreeting
        {
            Email = email,
            UserName = userName ?? string.Empty,
            Greeting = greeting ?? string.Empty
        };
    }
}
