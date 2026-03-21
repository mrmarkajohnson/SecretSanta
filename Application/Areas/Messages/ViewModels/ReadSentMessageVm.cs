using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.ViewModels;

public sealed class ReadSentMessageVm : ReadMessageVm, IReadSentMessage
{
    public IUserNamesBase? SentTo { get; set; }

    public override bool IsSentMessage
    {
        get => true;
        set { }
    }
}
