using Data.Entities.Santa;

namespace Data.Expressions;

public static class DbPartnerLinkExpressions
{

    public static Func<Santa_PartnerLink, bool> IsActive()
    {
        return x => x.DateDeleted == null && x.DateArchived == null;
    }
}
