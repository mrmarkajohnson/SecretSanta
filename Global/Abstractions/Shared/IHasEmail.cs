namespace Global.Abstractions.Shared;

public interface IHasEmail
{
    /// <summary>
    /// Only make this visible to the user, potential/actual partners and group admins
    /// </summary>
    string? Email { get; set; }

    bool ShowEmail { get; set; }
}

public static class HasEmailExtensions
{
    public static string EmailForDisplay(this IHasEmail user)
    {
        return user.ShowEmail ? (user.Email ?? string.Empty) : string.Empty;
    }
}
