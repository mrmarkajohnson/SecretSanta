using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

public class ReadMessage : SantaMessage, IReadMessage
{
    public ReadMessage()
    {
        PreviousMessages = new List<ISantaMessage>();
    }

    public int GiftingGroupKey { get; set; }

    public IList<ISantaMessage> PreviousMessages { get; set; }
}
