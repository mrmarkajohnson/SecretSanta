namespace Global.Names;

public static class StandardPhrases
{
    public const string DetailsScrambled = $"{UserDisplayNames.UserName}s and {UserDisplayNames.EmailLower}es are scrambled in the database, " +
        $"so that {UserDisplayNames.EmailLower}es can't be misused";

    public const string EmailVisibility = $"Unless you use your {UserDisplayNames.EmailLower} as your {UserDisplayNames.UserName}, " +
        "it is only visible to group administrators, current partners, and potential partners you suggest";

    public const string EmailOrUserNameExplanation = $"If you have provided an {UserDisplayNames.EmailLower} you can enter it here, " +
        $"or enter your {UserDisplayNames.UserName}";

    public const string EmailVisibleExplanation = $"users who view your details can see your {UserDisplayNames.EmailLower}";

    /// <summary>
    /// For select tags that may start empty
    /// </summary>
    public const string PleaseSelect = "Please select one...";
}
