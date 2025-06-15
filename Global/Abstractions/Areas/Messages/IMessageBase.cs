using static Global.Settings.MessageSettings;

namespace Global.Abstractions.Areas.Messages;

public interface IMessageBase
{
    MessageRecipientType RecipientType { get; }

    string HeaderText { get; }
    string MessageText { get; set; }

    bool Important { get; set; }
    bool CanReply { get; set; }
}
