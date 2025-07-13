namespace Data.Entities.Santa;

public class Santa_Message : MessageBaseEntity, IMessageEntity
{
    public Santa_Message()
    {
        Recipients = new HashSet<Santa_MessageRecipient>();
        Replies = new HashSet<Santa_Message>();
        RepliesToReplies = new HashSet<Santa_Message>();
    }

    public bool ShowAsFromSanta { get; set; }

    public int? GiftingGroupYearKey { get; set; }
    public virtual Santa_GiftingGroupYear? GiftingGroupYear { get; set; }

    public virtual required Santa_User Sender { get; set; }

    /// <summary>
    /// The message this one is replying to
    /// </summary>
    public int? ReplyToMessageKey { get; set; }
    public virtual Santa_Message? ReplyToMessage { get; set; }

    /// <summary>
    /// If replying to a reply, the original message that was a reply to (for the original sender name)
    /// Only set when someone replies to their own message
    /// </summary>
    public int? OriginalMessageKey { get; set; }
    public virtual Santa_Message? OriginalMessage { get; set; }

    public virtual ICollection<Santa_MessageRecipient> Recipients { get; set; }
    public virtual ICollection<Santa_Message> Replies { get; set; }

    /// <summary>
    /// The corresponding set for OriginalMessage
    /// Only set when someone replies to their own message
    /// </summary>
    public virtual ICollection<Santa_Message> RepliesToReplies { get; set; }
}
