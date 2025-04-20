using Application.Shared.Helpers;

namespace Global.Abstractions.Shared;

public interface IHasGlobalUserId
{
    /// <summary>
    /// Don't expose this in the front end, as it is used for encryption and decryption
    /// Use a hashed version instead - see IHasHashedUserId
    /// </summary>
    string GlobalUserId { get; }
}

public static class HasGlobalUserIdExtensions
{
    public static string GetHashedUserId(this IHasGlobalUserId user)
    {
        return EncryptionHelper.TwoWayEncrypt(user.GlobalUserId.ToString());
    }
}
