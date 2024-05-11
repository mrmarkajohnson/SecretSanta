namespace Global.Validation;

public static class UserDetails
{
    public static class Forename
    {
        public const int MinLength = 2;
        public const int MaxLength = 250;
    }

    public static class MiddleNames
    {
        public const int MaxLength = 250;
    }

    public static class Surname
    {
        public const int MinLength = 2;
        public const int MaxLength = 250;
    }

    public static class SecurityQuestions
    {
        public const int MinLength = 8;
        public const int MaxLength = 500;
    }

    public static class SecurityAnswers
    {
        public const int MinLength = 5;
        public const int MaxLength = 250;
    }

    public static class SecurityHints
    {
        public const int MaxLength = 250;
    }
}
