using Application.Shared.Helpers;

namespace Global.Helpers;

public static class UserHelper
{
    public static Guid GetGlobalUserId(string hashedUserId)
    {
        return Guid.Parse(EncryptionHelper.Decrypt(hashedUserId));
    }
}
