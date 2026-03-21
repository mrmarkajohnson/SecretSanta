using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.BaseModels;

public class SentMessage : SantaMessageBase, ISentMessage
{
    private IUserNamesBase? _sentTo;

    public override bool IsSentMessage
    {
        get => true;
        set { }
    }

    public IUserNamesBase? SentTo
    {
        get
        {
            if (!ShowAsToSanta && _sentTo != null && _sentTo.IdentificationHashed)
            {
                _sentTo.UnHash();
            }

            return _sentTo;
        }
        set => _sentTo = value;
    }
}
