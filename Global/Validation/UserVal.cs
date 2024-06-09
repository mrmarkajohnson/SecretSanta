namespace Global.Validation;

public static class UserVal
{
    public static class Forename
    {
        public const int MinLength = 2;
        public const int MaxLength = 80;
    }

    public static class MiddleNames
    {
        public const int MaxLength = 120;
    }

    public static class Surname
    {
        public const int MinLength = 2;
        public const int MaxLength = 100;
    }

    public static class SecurityQuestions
    {
        public const int MinLength = 8;
        public const int MaxLength = 350;
    }

    public static class SecurityAnswers
    {
        public const int MinLength = 5;
        public const int MaxLength = 200;
    }

    public static class SecurityHints
    {
        public const int MaxLength = 250;
    }
}
