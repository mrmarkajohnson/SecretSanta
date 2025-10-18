using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IUserGiftingGroup : IHaveAGroupKey
{
    string GroupName { get; }
    bool GroupAdmin { get; }
    int NewApplications { get; }
}
