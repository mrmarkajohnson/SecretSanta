using Global.Abstractions.Shared;
using static Global.Settings.GlobalSettings;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IReviewGroupInvitation : IGiftingGroupInvitationBase
{    
    int GiftingGroupKey { get; set; }
    int? ToSantaUserKey { get; }

    string GroupName { get; }
    string GroupDescription { get; }

    IUserNamesBase FromUser { get; }

    /// <summary>
    /// Not sure means come back to later
    /// </summary>
    YesNoNotSure? Accept { get; }

    string? RejectionMessage { get; }
}
