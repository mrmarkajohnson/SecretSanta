namespace Global.Abstractions.Areas.Messages;

public interface IHasMessageChain
{
    IList<ISantaMessage> PreviousMessages { get; set; }
    IList<ISantaMessage> LaterMessages { get; set; }
}
