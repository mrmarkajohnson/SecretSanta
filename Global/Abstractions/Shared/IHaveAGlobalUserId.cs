using Global.Helpers;

namespace Global.Abstractions.Shared;

public interface IHaveAGlobalUserId
{
    /// <summary>
    /// Don't expose this in the front end, as it is used for encryption and decryption
    /// Use a hashed version instead - see IHasHashedUserId
    /// </summary>
    string GlobalUserId { get; }
}

public static class HaveAGlobalUserIdExtensions
{
    public static string GetHashedUserId(this IHaveAGlobalUserId user)
    {
        return EncryptionHelper.TwoWayEncrypt(user.GlobalUserId.ToString());
    }
}
