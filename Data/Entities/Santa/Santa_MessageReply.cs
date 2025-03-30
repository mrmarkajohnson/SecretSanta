using Data.Entities.Shared.Base;

namespace Data.Entities.Santa;

public class Santa_MessageReply : BaseEntity, IMessageReply
{
    [Key]
    public int MessageReplyKey { get; set; }

    public int OriginalMessageKey { get; set; }
    public virtual required Santa_Message OriginalMessage { get; set; }

    public int ReplyMessageKey { get; set; }
    public virtual required Santa_Message ReplyMessage { get; set; }
}
