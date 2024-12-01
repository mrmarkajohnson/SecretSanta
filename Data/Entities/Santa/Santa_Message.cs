namespace Data.Entities.Santa;

public class Santa_Message : MessageBaseEntity, IMessage
{
    public Santa_Message()
    {
        Recipients = new HashSet<Santa_MessageRecipient>();
        Replies = new HashSet<Santa_MessageReply>();
    }

    public virtual required Santa_YearGroupUser Sender { get; set; }
    public virtual Santa_MessageReply? ReplyTo { get; set; }

    // TODO: Allow the sender to be the system (make Sender optional, or have a bool to say 'system' instead)
    // TODO: Allow messages to be set as 'Important' so the message alert will be highlighted

    public virtual ICollection<Santa_MessageRecipient> Recipients { get; set; }
    public virtual ICollection<Santa_MessageReply> Replies { get; set; }
}
