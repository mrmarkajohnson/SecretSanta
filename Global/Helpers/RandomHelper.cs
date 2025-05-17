namespace Global.Helpers;

public static class RandomHelper
{
    private const string _alphabetUppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string _alphabetLowercase = _alphabetUppercase.ToLower();
    private const string _numbers = "0123456789";

    public static string RandomAlphanumericCharacters(int length)
    {
        var allCharacters = _alphabetUppercase + _alphabetLowercase + _numbers;
        List<char> randomNCharacters = allCharacters.GetNFromList<char>(length);
        return string.Join(' ', randomNCharacters).Replace(" ", string.Empty);
    }
}
