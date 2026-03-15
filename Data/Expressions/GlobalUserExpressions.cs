using Data.Entities.Shared;

namespace Data.Expressions;

public static class GlobalUserExpressions
{
    public static Func<Global_User, bool> IsActive()
    {
        return x => x.SantaUser != null && x.SantaUser.DateArchived == null;
    }
}
