using Data.Entities.Shared.Base;

namespace Data.Entities.Santa;

public class Santa_MessageRecipient : ArchivableBaseEntity, IArchivableEntity
{
    [Key]
    public int Id { get; set; }

    public int MessageId { get; set; }
    public virtual required Santa_Message Message { get; set; }

    public int RecipientId { get; set; }
    public virtual required Santa_GiftingGroupUser Recipient { get; set; }

    public bool Read { get; set; }
}
