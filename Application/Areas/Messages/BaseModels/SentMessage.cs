using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

public class SentMessage : SantaMessageBase, ISentMessage
{
    public override bool IsSentMessage
    {
        get => true;
        set { }
    }
}
