using Global.Abstractions.Global;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IUserGiftingGroupYear : IGiftingGroupYearBase, IUserGroupYearShared
{
    bool GroupAdmin { get; }
    IUserNamesBase? Recipient { get; }

    string LimitString { get; }
    string RecipientString { get; }
}
