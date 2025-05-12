using Global.Helpers;

namespace Global.Abstractions.Shared;

public interface IHasHashedUserId
{
    string HashedUserId { get; }
}

public static class HasHashedUserIdExtensions
{
    public static Guid? GetGlobalUserId(this IHasHashedUserId user)
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

    public static string? GetStringUserId(this IHasHashedUserId user)
    {
        return user.GetGlobalUserId()?.ToString();
    }
}
