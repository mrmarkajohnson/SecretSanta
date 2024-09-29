namespace Global.Validation;

public static class GiftingGroupVal
{
    public static class Name
    {
        public const int MinLength = 4;
        public const int MaxLength = 150;
    }

    public static class Description
    {
        public const int MinLength = 6;
        public const int MaxLength = 250;
    }

    public static class JoinerToken
    {
        public const int MinLength = 8;
        public const int MaxLength = 20;
    }

    public static class CultureInfo
    {
        public const int MinLength = 2;
        public const int MaxLength = 8;
    }

    public static class CurrencyCodeOverride
    {
        public const int MaxLength = 4;
    }

    public static class CurrencySymbolOverride
    {
        public const int MaxLength = 3;
    }

    public static class JoinerMessage
    {
        public const int MaxLength = 400;
    }
}
