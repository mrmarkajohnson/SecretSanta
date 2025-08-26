using static Global.Settings.MessageSettings;

namespace Global.Helpers;

public static class MessageHelper
{
    public static string MessageRecipientToDescription(this MessageRecipientType recipientType, string? groupName = null)
    {
        if (recipientType is MessageRecipientType.OriginalSender or MessageRecipientType.PotentialPartner
                or MessageRecipientType.SingleGroupMember or MessageRecipientType.SingleNonGroupMember)
        {
            return "You";
        }

        return recipientType switch
        {
            MessageRecipientType.GiftRecipient => "You (as gift recipient)",
            MessageRecipientType.Gifter => "You (as gift giver)",
            _ => TypeNameWithGroup(recipientType, groupName)
        };
    }

    public static string SenderToDescription(this MessageRecipientType recipientType, string? groupName = null, bool otherAdmins = false)
    {
        return TypeNameWithGroup(recipientType, groupName, otherAdmins);
    }

    public static string SenderToDescription(this MessageRecipientType recipientType, string? groupName, string? recipientName)
    {
        string description = TypeNameWithGroup(recipientType, groupName);

        if (recipientName.IsNotEmpty())
        {
            if (recipientType >= MessageRecipientType.OriginalSender && recipientType <= MessageRecipientType.OriginalAllEverRecipients)
            {
                description = description.Replace(" sender", $" sender ({recipientName})");
            }
            else if (recipientType == MessageRecipientType.GiftRecipient)
            {
                description = description + $" ({recipientName})";
            }
        }

        return description;
    }

    public static string TypeNameWithGroup(this MessageRecipientType recipientType, string? groupName, bool otherAdmins = false)
    {
        string displayName = recipientType.DisplayName();

        if (groupName.IsNotEmpty())
        {
            string fullGroup = $"'{groupName}'";
            return displayName
                .Replace("the group", fullGroup)
                .Replace("group", fullGroup, StringComparison.InvariantCultureIgnoreCase);
        }

        if (otherAdmins && recipientType == MessageRecipientType.GroupAdmins)
        {
            displayName = "Other " + displayName;
        }

        return displayName;
    }

    public static Dictionary<MessageRecipientType, MessageRecipientType> FutureRecipientSwitches => new Dictionary<MessageRecipientType, MessageRecipientType>
    {
        { MessageRecipientType.YearGroupCurrentMembers, MessageRecipientType.YearGroupAllEverMembers },
        { MessageRecipientType.GroupCurrentMembers,  MessageRecipientType.GroupAllEverMembers },
        { MessageRecipientType.OriginalCurrentRecipients, MessageRecipientType.OriginalAllEverRecipients }
    };

    public static bool SpecificMember(this MessageRecipientType recipientType)
    {
        return recipientType == MessageRecipientType.SingleGroupMember;
    }

    public static bool AllowsFutureViewing(this MessageRecipientType recipientType)
    {
        return recipientType.ToString().Contains("AllEver")
            || recipientType is MessageRecipientType.GiftRecipient or MessageRecipientType.Gifter;
    }

    public static string FutureLabel(this MessageRecipientType recipientType, MessageRecipientType? originalType = null)
    {
        return recipientType switch // the label goes on the corresponding 'Current' type
        {
            MessageRecipientType.YearGroupCurrentMembers 
                => "Include Future Partipicipants",
            MessageRecipientType.GroupCurrentMembers 
                => "Include Future Members",
            MessageRecipientType.OriginalCurrentRecipients 
                => originalType is MessageRecipientType.YearGroupAllEverMembers or MessageRecipientType.GroupAllEverMembers
                    ? "Include Future Recipients"
                    : string.Empty,
            _ => string.Empty
        };
    }

    public static string FutureExplanation(this MessageRecipientType recipientType, MessageRecipientType? originalType = null)
    {
        return recipientType switch // the explanation goes on the corresponding 'Current' type
        {
            MessageRecipientType.YearGroupCurrentMembers
                => "Make this visible to any future partipants, who are included later?",
            MessageRecipientType.GroupCurrentMembers
                => "Make this visible to any future members, who are added to the group later?",
            MessageRecipientType.OriginalCurrentRecipients
                => originalType switch
                {
                    MessageRecipientType.YearGroupAllEverMembers =>
                        "Make this visible to any future recipients of the original message, i.e. future partipants included later?",
                    MessageRecipientType.GroupAllEverMembers =>
                        "Make this visible to any future recipients of the original message, i.e. group members added later?",
                    _ => string.Empty
                },
            _ => string.Empty
        };
    }
}
