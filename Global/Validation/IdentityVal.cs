using Microsoft.AspNetCore.Identity;

namespace Global.Validation;

public static class IdentityVal
{
    public static class Passwords
    {
        public const int MinLength = 8;
        public const int MaxLength = 100;
        public const int UniqueChars = 2;
        public const bool NonAlphanumeric = true;
        public const bool Lowercase = false;
        public const bool Uppercase = false;
        public const bool Digit = true;

        public static string Description = $"Must be at least {MinLength} characters, with at least one "
            + (Digit ? "digit, " : "")
            + (NonAlphanumeric ? "symbol, " : "")
            + (Lowercase ? "lowercase, " : "")
            + (Uppercase ? "uppercase, " : "")
            + $"and {UniqueChars} unique characters.";
    }

    public static class Lockouts
    {
        public static TimeSpan DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
        public const int MaxFailedAccessAttempts = 5;
        public const bool AllowedForNewUsers = true;
    }

    public static class UserNames
    {
        public const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        public const int MinLength = 8;
        public const int MaxLength = 100;
    }

    public static class Emails
    {
        public const bool Unique = false;
    }

    public static class SignIn
    {
        public static bool RequireConfirmedEmail = false;
        public static bool RequireConfirmedPhoneNumber = false;
        public static bool RequireConfirmedAccount = false;
    }

    public static void ConfigureOptions(IdentityOptions options)
    {
        // Password settings.
        options.Password.RequireDigit = Passwords.Digit;
        options.Password.RequireLowercase = Passwords.Lowercase;
        options.Password.RequireNonAlphanumeric = Passwords.NonAlphanumeric;
        options.Password.RequireUppercase = Passwords.Uppercase;
        options.Password.RequiredLength = Passwords.MinLength;
        options.Password.RequiredUniqueChars = Passwords.UniqueChars;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = Lockouts.DefaultLockoutTimeSpan;
        options.Lockout.MaxFailedAccessAttempts = Lockouts.MaxFailedAccessAttempts;
        options.Lockout.AllowedForNewUsers = Lockouts.AllowedForNewUsers;

        // User settings.
        options.User.AllowedUserNameCharacters = UserNames.Characters;
        options.User.RequireUniqueEmail = Emails.Unique;
    }
}
