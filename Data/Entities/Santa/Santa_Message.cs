using Data.Entities.Shared.Base;

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

    public virtual ICollection<Santa_MessageRecipient> Recipients { get; set; }
    public virtual ICollection<Santa_MessageReply> Replies { get; set; }
}
