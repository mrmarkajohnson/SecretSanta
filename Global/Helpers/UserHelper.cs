using Application.Shared.Helpers;

namespace Global.Helpers;

public static class UserHelper
{
    public static Guid? GetGlobalUserId(string hashedUserId)
    {
        if (string.IsNullOrWhiteSpace(hashedUserId))
            return null;
        
        return Guid.Parse(EncryptionHelper.Decrypt(hashedUserId));
    }
}
