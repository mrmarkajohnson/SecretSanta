using Global.Abstractions.Global;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IManageUserGiftingGroupYear : IUserGiftingGroupYear
{
    int PreviousYearsRequired { get; set; }

    string? LastYearRecipientId { get; set; }
    string? PreviousYearRecipientId { get; set; }

    IList<IUserNamesBase> OtherGroupMembers { get; set; }    
}
