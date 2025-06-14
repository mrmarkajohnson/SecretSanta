using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Participate;

public interface IManageUserGiftingGroupYear : IUserGiftingGroupYear
{
    int PreviousYearsRequired { get; set; }

    string? LastRecipientUserId { get; set; }
    string? PreviousRecipientUserId { get; set; }

    IList<IUserNamesBase> OtherGroupMembers { get; set; }
}
