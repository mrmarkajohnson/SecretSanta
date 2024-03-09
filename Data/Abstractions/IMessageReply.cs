namespace Data.Abstractions;

public interface IMessageReply : IEntity
{
    int OriginalMessageId { get; set; }
    int ReplyMessageId { get; set; }
}
