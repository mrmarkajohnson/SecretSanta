using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

public sealed class ReadSentMessage : SentMessage, IReadSentMessage
{
    public ReadSentMessage()
    {
        PreviousMessages = new List<ISantaMessage>();
        LaterMessages = new List<ISantaMessage>();
    }

    public int GiftingGroupKey { get; set; }
    public string SenderName => ShowAsFromSanta ? "You (as Santa)" : "You";

    public IList<ISantaMessage> PreviousMessages { get; set; }
    public IList<ISantaMessage> LaterMessages { get; set; }

    int ISantaMessage.MessageRecipientKey => 0;
    bool ISantaMessage.Read { get; set; }
    IUserNamesBase? ISantaMessage.Sender => null;
}
