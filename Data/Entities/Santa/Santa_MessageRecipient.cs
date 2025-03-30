namespace Data.Entities.Santa;

public class Santa_MessageRecipient : ArchivableBaseEntity, IArchivableEntity
{
    [Key]
    public int MessageRecipientKey { get; set; }

    public int MessageKey { get; set; }
    public virtual required Santa_Message Message { get; set; }

    public int RecipientSantaUserKey { get; set; }
    public virtual required Santa_User RecipientSantaUser { get; set; }

    public bool Read { get; set; }
}
