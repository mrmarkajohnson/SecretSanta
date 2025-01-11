namespace Global.Abstractions.Global.Messages;

public interface IReadMessage : IMessageBase
{
    int RecipientId { get; }
    IUserNamesBase? Sender { get; }
    bool Read { get; set; }
    bool ShowAsFromSanta { get; }
    string SenderName { get; }
}
