using Data.Entities.Santa;

namespace Data.Expressions;

public static class GroupInvitationExpressions
{
    public static Func<Santa_Invitation, bool> IsActive(bool checkGroup)
    {
        if (checkGroup)
        {
            return x => x.DateArchived == null && x.GiftingGroup.DateArchived == null;
        }
        else
        {
            return x => x.DateArchived == null;
        }
    }
}

