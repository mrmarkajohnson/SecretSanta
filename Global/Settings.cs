namespace Global;

public static class Settings
{
    public enum MessageRecipientType
    {
        SystemAdmins = 0,
        GroupAdmins = 1,
        GiftRecipient = 2,
        Gifter = 3,
        YearGroupCurrentMembers = 4,
        YearGroupAllMembers = 5,
        GroupAllCurrentMembers = 6,
        GroupAllEverMembers = 7,
        OriginalSender = 8,
        OriginalCurrentRecipients = 9,
        OriginalAllEverRecipients = 10
    }

    public static List<MessageRecipientType> OriginalOnlyRecipientTypes = new List<MessageRecipientType>
    {
        MessageRecipientType.GiftRecipient, MessageRecipientType.Gifter, MessageRecipientType.YearGroupCurrentMembers, 
        MessageRecipientType.YearGroupAllMembers, MessageRecipientType.GroupAllCurrentMembers, MessageRecipientType.GroupAllEverMembers
    };

    public static List<MessageRecipientType> ReplyOnlyRecipientTypes = new List<MessageRecipientType>
    { 
        MessageRecipientType.OriginalSender, MessageRecipientType.OriginalCurrentRecipients, MessageRecipientType.OriginalAllEverRecipients
    };

    public const string SymmetricKeyStart = "kj*8%u803wq&*&^*sdf&w4w4eq9"; // DO NOT change this once used!

    public const string StandardEmailEnd = "@secretsanta.com";
}
