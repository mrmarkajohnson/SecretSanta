using Global.Helpers;

namespace Global.Abstractions.Areas.Messages;

/// <summary>
/// Content shared between sent and received messages
/// </summary>
public interface ISantaMessageShared: ISantaMessageBase
{
    bool IsSentMessage { get; set; }
    string? ReplyToName { get; }
    string? SpecificRecipientName { get; }
}

public static class SantaMessageSharedExtensions
{
    public static string ToDescription(this ISantaMessageShared message)
    {
        return message.IsSentMessage
            ? SenderToDescription(message)
            : RecipientToDescription(message);
    }

    public static string SenderToDescription(this ISantaMessageShared message)
    {
        return message.RecipientType.SenderToDescription(message.GroupName, 
            message.ReplyToName ?? message.SpecificRecipientName);
    }

    public static string RecipientToDescription(this ISantaMessageShared message)
    {
        return message.RecipientType.MessageRecipientToDescription(message.GroupName);
    }
}