using System.ComponentModel.DataAnnotations;

namespace Global.Settings;

public static class MessageSettings
{
    public const string FromMessageParameter = "fromMessage";
    public const string FromRecipientParameter = "recipient";

    public static string? ViewMessageUrl => ConfigurationSettings.BaseUrl.IsNotEmpty()
        ? ConfigurationSettings.BaseUrl + "/Messages/Home/ViewMessages"
        : null;

    public static string? ConfirmEmailUrl => ConfigurationSettings.BaseUrl.IsNotEmpty()
        ? ConfigurationSettings.BaseUrl + "/Account/Manage/ConfirmEmail"
        : null;

    public static string? EmailPreferencesUrl => ConfigurationSettings.BaseUrl.IsNotEmpty()
        ? ConfigurationSettings.BaseUrl + "/Account/Manage/EmailPreferences"
        : null;

    public enum MessageRecipientType
    {
        /// <summary>
        /// Systen administrators - not yet a thing, but for future use
        /// </summary>
        [Display(Name = "System Administrators")]
        SystemAdmins = 0,

        /// <summary>
        /// Administrators of the group
        /// </summary>
        [Display(Name = "Group Administrators")]
        GroupAdmins = 1,

        /// <summary>
        /// To the person the sender is giving to
        /// </summary>
        [Display(Name = "Your gift recipient for the group")]
        GiftRecipient = 2,

        /// <summary>
        /// To the person giving to the sender
        /// </summary>
        [Display(Name = "Your gift giver for the group")]
        Gifter = 3,

        /// <summary>
        /// To current participants in the group for this year
        /// </summary>
        [Display(Name = "All group participants this year")]
        YearGroupCurrentMembers = 4,

        /// <summary>
        /// Visible to anyone who participates in group this year, including those not included yet
        /// </summary>
        [Display(Name = "All group participants this year")]
        YearGroupAllEverMembers = 5,

        /// <summary>
        /// To current members of the group
        /// </summary>
        [Display(Name = "All other group members")]
        GroupCurrentMembers = 6,

        /// <summary>
        /// Visible to anyone who joins the group in future
        /// </summary>
        [Display(Name = "All other group members")]
        GroupAllEverMembers = 7,

        /// <summary>
        /// Direct reply only to the sender
        /// </summary>
        [Display(Name = "The original sender")]
        OriginalSender = 8,

        /// <summary>
        /// Reply to all current recipients of the original message
        /// </summary>
        [Display(Name = "The sender and all recipients")]
        OriginalCurrentRecipients = 9,

        /// <summary>
        /// Reply visible to any future recipients of the original, e.g. if sent to all group members
        /// </summary>
        [Display(Name = "The sender and all recipients")]
        OriginalAllEverRecipients = 10,
        
        /// <summary>
        /// For automated messages about suggested relationships
        /// </summary>
        [Display(Name = "You")]
        PotentialPartner = 11,

        /// <summary>
        /// A single current member of the group
        /// </summary>
        [Display(Name = "A specific member of the group")]
        SingleGroupMember = 12,

        /// <summary>
        /// For automated messages, e.g. for failed group applications
        /// </summary>
        [Display(Name = "A specific person not in the group")]
        SingleNonGroupMember = 13,

        /// <summary>
        /// Not set yet - when sending a message (equivalent to null)
        /// </summary>
        TBC = 999
    }

    public static List<MessageRecipientType> OriginalRecipientTypes = new List<MessageRecipientType>
    {
        MessageRecipientType.GroupAdmins, MessageRecipientType.GiftRecipient, MessageRecipientType.Gifter, 
        MessageRecipientType.YearGroupCurrentMembers, MessageRecipientType.GroupCurrentMembers,
        MessageRecipientType.SingleGroupMember,
        // MessageRecipientType.YearGroupAllEverMembers, MessageRecipientType.GroupAllEverMembers // handled elsewhere
    };

    public static List<MessageRecipientType> ReplyRecipientTypes = new List<MessageRecipientType>
    {
        MessageRecipientType.OriginalSender, MessageRecipientType.OriginalCurrentRecipients, 
        // MessageRecipientType.OriginalAllEverRecipients // handled elsewhere
    };

    public static List<MessageRecipientType> SpecificRecipientTypes = new List<MessageRecipientType>
    {
        MessageRecipientType.GiftRecipient, MessageRecipientType.SingleGroupMember, MessageRecipientType.SingleNonGroupMember
    };

    public enum EmailPreference
    {
        None = 0,

        [Display(Name = "Important Only")]
        ImportantOnly = 1,

        All = 2
    }
}
