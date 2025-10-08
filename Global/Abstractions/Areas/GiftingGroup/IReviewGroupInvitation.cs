using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IReviewGroupInvitation : IGiftingGroupInvitationBase
{    
    IHashableUser FromUser { get; }
    int? ToSantaUserKey { get; }
    
    /// <summary>
    /// Null means come back to later
    /// </summary>
    bool? Accept { get; }
}
