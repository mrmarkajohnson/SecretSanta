using static Global.Settings.MessageSettings;

namespace Data.Entities.Shared.Base;

/// <summary>
/// Allows expansion using the same database and users; each message key will be unique
/// </summary>
public abstract class MessageBaseEntity : ArchivableBaseEntity, IMessageEntity
{
    [Key]
    public int MessageKey { get; set; }

    public int SenderKey { get; set; }

    public MessageRecipientType RecipientType { get; set; }

    [MaxLength(200)]
    public required string HeaderText { get; set; }

    public required string MessageText { get; set; }

    public bool Important { get; set; }
    public bool CanReply { get; set; }
}
