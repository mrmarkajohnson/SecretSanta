using Data.Entities.Santa;

namespace Data.Expressions;

public static class SantaUserExpressions
{
    public static Func<Santa_User, bool> IsActive()
    {
        return x => x.DateArchived == null;
    }
}
