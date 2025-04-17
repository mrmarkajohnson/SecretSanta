using static Global.Settings.GlobalSettings;

namespace Global.Helpers;

public static class GenderHelper
{
    public static string Direct(this Gender gender, bool capitalise = false)
    {
        return gender switch
        {
            Gender.Female => capitalise ? "She" : "she",
            Gender.Male => capitalise ? "He" : "he",
            _ => capitalise ? "They" : "they"
        };
    }

    public static string Indirect(this Gender gender, bool capitalise = false)
    {
        return gender switch
        {
            Gender.Female => capitalise ? "Her" : "her",
            Gender.Male => capitalise ? "Him" : "him",
            _ => capitalise ? "Them" : "them"
        };
    }

    public static string Posessive(this Gender gender, bool capitalise = false)
    {
        return gender switch
        {
            Gender.Female => capitalise ? "Her" : "her",
            Gender.Male => capitalise ? "His" : "his",
            _ => capitalise ? "Their" : "their"
        };
    }

    public static string IsShort(this Gender gender, bool capitalise = false)
    {
        return gender switch
        {
            Gender.Female => capitalise ? "She's" : "she's",
            Gender.Male => capitalise ? "He's" : "he's",
            _ => capitalise ? "They're" : "they're"
        };
    }

    public static string IsLong(this Gender gender, bool capitalise = false)
    {
        return gender switch
        {
            Gender.Female => capitalise ? "She is" : "she is",
            Gender.Male => capitalise ? "He is" : "he is",
            _ => capitalise ? "They are" : "they are"
        };
    }
}
