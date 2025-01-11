using System.ComponentModel.DataAnnotations;

namespace Global.Settings;

public static class MessageSettings
{
    public enum MessageRecipientType
    {
        [Display(Name = "System Administrators")]
        SystemAdmins = 0,
        [Display(Name = "Group Administrators")]
        GroupAdmins = 1,
        [Display(Name = "You")]
        GiftRecipient = 2,
        [Display(Name = "You")]
        Gifter = 3,
        [Display(Name = "Group Members")]
        YearGroupCurrentMembers = 4,
        [Display(Name = "Group Members")]
        YearGroupAllMembers = 5,
        [Display(Name = "Group Members")]
        GroupAllCurrentMembers = 6,
        [Display(Name = "Group Members")]
        GroupAllEverMembers = 7,
        [Display(Name = "You")]
        OriginalSender = 8,
        [Display(Name = "You")]
        OriginalCurrentRecipients = 9,
        [Display(Name = "You")]
        OriginalAllEverRecipients = 10,
        [Display(Name = "You")]
        PotentialPartner = 11
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
