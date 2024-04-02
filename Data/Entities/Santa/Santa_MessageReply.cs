using Data.Entities.Shared.Base;

namespace Data.Entities.Santa;

public class Santa_MessageReply : BaseEntity, IMessageReply
{
    [Key]
    public int Id { get; set; }

    public int OriginalMessageId { get; set; }
    public virtual required Santa_Message OriginalMessage { get; set; }

    public int ReplyMessageId { get; set; }
    public virtual required Santa_Message ReplyMessage { get; set; }
}
