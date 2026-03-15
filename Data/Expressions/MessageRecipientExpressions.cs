using Data.Entities.Santa;

namespace Data.Expressions;

public class MessageRecipientExpressions
{
    public static Func<Santa_MessageRecipient, bool> IsActive()
    {
        return x => x.DateArchived == null && x.Message.DateArchived == null;
    }
}
