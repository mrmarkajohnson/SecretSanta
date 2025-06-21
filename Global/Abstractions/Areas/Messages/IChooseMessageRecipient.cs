using Global.Abstractions.Areas.GiftingGroup;

namespace Global.Abstractions.Areas.Messages;

public interface IChooseMessageRecipient : IHasMessageRecipientType
{
    int? ReplyToMessageKey { get; }
    bool IncludeFutureMembers { get; }
    int? SpecificGroupMemberKey { get; }
    int? GiftingGroupKey { get; }

    IList<IUserGiftingGroup> GiftingGroups { get; }
}
