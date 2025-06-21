namespace Global.Abstractions.Areas.Messages;

public interface IMessageBase : IHasMessageRecipientType
{
    string HeaderText { get; }
    string MessageText { get; set; }

    bool Important { get; set; }
    bool CanReply { get; set; }
}
