using static Global.Settings.MessageSettings;

namespace Data.Entities.Shared.Base;

public abstract class MessageBaseEntity : ArchivableBaseEntity, IMessageEntity
{
    [Key]
    public int Id { get; set; }

    public int SenderId { get; set; }

    public MessageRecipientType RecipientTypes { get; set; }

    [MaxLength(200)]
    public required string HeaderText { get; set; }

    public required string MessageText { get; set; }

    public bool Important { get; set; }
}
