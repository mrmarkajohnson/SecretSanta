namespace Application.Shared.Helpers;

public static class EmailHelper
{
    public static bool IsEmail(string userNameOrEmail)
    {
        return userNameOrEmail.Contains("@") && userNameOrEmail.Contains(".");
    }
}
