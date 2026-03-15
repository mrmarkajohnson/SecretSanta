using Data.Entities.Santa;

namespace Data.Expressions;

public static class MessageExpressions
{
    public static Func<Santa_Message, bool> IsActive()
    {
        return x => x.DateArchived == null;
    }
}
