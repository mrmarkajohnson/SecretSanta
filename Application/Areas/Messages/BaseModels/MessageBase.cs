using Global.Abstractions.Global.Messages;
using Global.Settings;

namespace Application.Areas.Messages.BaseModels;

public class MessageBase : IMessageBase
{
    public required MessageSettings.MessageRecipientType RecipientTypes { get; set; }
    public required string HeaderText { get; set; }
    public required string MessageText { get; set; }
    public bool Important { get; set; }
}
