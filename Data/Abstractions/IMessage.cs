using static Global.Settings.MessageSettings;

namespace Data.Abstractions;

public interface IMessage : IArchivableEntity
{
    int Id { get; set; }

    int SenderId { get; set; }

    MessageRecipientType RecipientTypes { get; set; }

    string? HeaderText { get; set; }

    string MessageText { get; set; }

    bool Important { get; set; }
}
