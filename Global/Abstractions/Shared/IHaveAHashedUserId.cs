using Global.Helpers;

namespace Global.Abstractions.Shared;

public interface IHaveAHashedUserId
{
    string HashedUserId { get; }
}

public static class HaveAHashedUserIdExtensions
{
    public static Guid? GetGlobalUserId(this IHaveAHashedUserId user)
    {
        try
        {
            return UserHelper.GetGlobalUserId(user.HashedUserId);
        }
        catch
        {
            return null;
        }
    }

    public static string? GetStringUserId(this IHaveAHashedUserId user)
    {
        return user.GetGlobalUserId()?.ToString();
    }
}
