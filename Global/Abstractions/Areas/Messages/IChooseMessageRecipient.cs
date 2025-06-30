using Global.Abstractions.Areas.GiftingGroup;
using static Global.Helpers.MessageHelper;

namespace Global.Abstractions.Areas.Messages;

public interface IChooseMessageRecipient : IHasMessageRecipientType
{
    int? ReplyToMessageKey { get; }
    bool IncludeFutureMembers { get; set; }
    int? SpecificGroupMemberKey { get; }
    int? GiftingGroupKey { get; }

    IList<IUserGiftingGroup> GiftingGroups { get; }
}

public static class ChooseMessageRecipientExtensions
{
    public static void SetActualRecipientType(this IChooseMessageRecipient item)
    {
        if (item.IncludeFutureMembers && FutureRecipientSwitches.ContainsKey(item.RecipientType))
        {
            item.RecipientType = FutureRecipientSwitches[item.RecipientType];
        }
    }

    public static void SetDisplayRecipientType(this IChooseMessageRecipient item)
    {
        if (FutureRecipientSwitches.Any(x => x.Value == item.RecipientType))
        {
            item.RecipientType = FutureRecipientSwitches.First(x => x.Value == item.RecipientType).Key;
            item.IncludeFutureMembers = true;
        }
    }
}
