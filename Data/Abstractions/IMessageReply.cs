namespace Data.Abstractions;

public interface IMessageReply : IEntity
{
    int OriginalMessageKey { get; set; }
    int ReplyMessageKey { get; set; }
}
