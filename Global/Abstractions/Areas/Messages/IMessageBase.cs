using static Global.Settings.MessageSettings;

namespace Global.Abstractions.Global.Messages;

public interface IMessageBase
{
    MessageRecipientType RecipientTypes { get; }
    string HeaderText { get; }
    string MessageText { get; set; }
    bool Important { get; set; }
}
