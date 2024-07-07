namespace Global.Settings;

public static class MessageSettings
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
}
