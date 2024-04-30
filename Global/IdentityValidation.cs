using Microsoft.AspNetCore.Identity;

namespace Global;

public static class IdentityValidation
{
    public static class PasswordOptions
    {
        public const int RequiredLength = 8;
        public const int RequiredUniqueChars = 2;
        public const bool RequireNonAlphanumeric = true;
        public const bool RequireLowercase = false;
        public const bool RequireUppercase = false;
        public const bool RequireDigit = true;

        public static string Description = $"Must be at least {RequiredLength} characters, with at least one "
            + (RequireDigit ? "digit, " : "")
            + (RequireNonAlphanumeric ? "symbol, " : "")
            + (RequireLowercase ? "lowercase, " : "")
            + (RequireUppercase ? "uppercase, " : "")
            + $"and {RequiredUniqueChars} unique characters.";
    }

    public static class LockoutOptions
    {
        public static TimeSpan DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
        public const int MaxFailedAccessAttempts = 5;
        public const bool AllowedForNewUsers = true;
    }

    public static class UserOptions
    {
        public const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        public const bool RequireUniqueEmail = false;
        public const int UserNameRequiredLength = 8;
    }

    public static class SignInOptions
    {
        public static bool RequireConfirmedEmail = false;
        public static bool RequireConfirmedPhoneNumber = false;
        public static bool RequireConfirmedAccount = false;
    }

    public static void ConfigureOptions(IdentityOptions options)
    {
        // Password settings.
        options.Password.RequireDigit = PasswordOptions.RequireDigit;
        options.Password.RequireLowercase = PasswordOptions.RequireLowercase;
        options.Password.RequireNonAlphanumeric = PasswordOptions.RequireNonAlphanumeric;
        options.Password.RequireUppercase = PasswordOptions.RequireUppercase;
        options.Password.RequiredLength = PasswordOptions.RequiredLength;
        options.Password.RequiredUniqueChars = PasswordOptions.RequiredUniqueChars;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = LockoutOptions.DefaultLockoutTimeSpan;
        options.Lockout.MaxFailedAccessAttempts = LockoutOptions.MaxFailedAccessAttempts;
        options.Lockout.AllowedForNewUsers = LockoutOptions.AllowedForNewUsers;

        // User settings.
        options.User.AllowedUserNameCharacters = UserOptions.AllowedUserNameCharacters;
        options.User.RequireUniqueEmail = UserOptions.RequireUniqueEmail;
    }
}
