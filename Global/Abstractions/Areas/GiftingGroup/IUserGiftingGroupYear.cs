using Global.Abstractions.Global;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IUserGiftingGroupYear : IGiftingGroupYearBase
{
    string GiftingGroupName { get; }
    bool GroupAdmin { get; }
    bool Included { get; set; }
    IUserNamesBase? Recipient { get; set; }

    string LimitString { get; }
    string RecipientString { get; }
}
