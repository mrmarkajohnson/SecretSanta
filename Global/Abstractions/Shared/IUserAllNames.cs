using static Global.Settings.GlobalSettings;

namespace Global.Abstractions.Shared;

public interface IUserAllNames
{
    string Forename { get; set; }
    string? MiddleNames { get; set; }
    string? PreferredFirstName { get; set; }
    bool PreferredIsNickname { get; set; }
    string Surname { get; set; }
    Gender Gender { get; set; }

    string UserDisplayName { get; }
}

public static class UserAllNamesExtensions
{
    public static string DisplayName(this IUserAllNames user, bool includeMiddleNames = true)
    {
        string fullName = string.IsNullOrWhiteSpace(user.PreferredFirstName) 
            ? user.Forename.Tidy() 
            : user.PreferredFirstName.Tidy();

        string? middleNames = user.MiddleNames.Tidy(false);

        includeMiddleNames = includeMiddleNames && middleNames.NotEmpty() && !fullName.ContainsWord(middleNames);

        if (includeMiddleNames)
        {
            if (middleNames != null && middleNames.ContainsWord(fullName))
            {
                middleNames = middleNames.Replace(fullName + " ", "").Replace(" " + fullName, ""); // avoid repeating preferred name
            }
            
            fullName += " " + middleNames.Tidy();
        }

        fullName += " " + user.Surname;
        return fullName.Tidy();
    }

    public static string FullName(this IUserAllNames user)
    {
        string foreName = user.Forename.Trim();
        string? middleNames = user.MiddleNames.Tidy(false);

        string? preferredName = IncludePreferredName(user.PreferredFirstName, foreName, middleNames)
            ? user.PreferredFirstName?.Trim()
            : null;

        string fullName = foreName + " ";

        if (preferredName.NotEmpty())
        {
            fullName += (user.PreferredIsNickname ? $"'{preferredName}'" : preferredName) + " ";
        }

        if (middleNames.NotEmpty())
        {
            fullName += middleNames.Trim() + " ";
        }

        fullName += user.Surname;
        return fullName.Tidy();
    }

    private static bool IncludePreferredName(string? preferredFirstName, string foreName, string? middleNames)
    {
        if (string.IsNullOrWhiteSpace(preferredFirstName))
            return false;

        preferredFirstName = preferredFirstName.Trim();

        if (foreName == preferredFirstName
            || middleNames == preferredFirstName
            || middleNames.ContainsWord(preferredFirstName))
        {
            return false;
        }

        return true;
    }

    public static string DisplayFirstName(this IUserAllNames user)
    {
        return string.IsNullOrWhiteSpace(user.PreferredFirstName) ? user.Forename : user.PreferredFirstName;
    }
}
