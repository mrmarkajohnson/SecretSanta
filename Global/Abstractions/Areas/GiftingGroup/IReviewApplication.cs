using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IReviewApplication : IHashableUser, IUserNamesBase
{
    int GroupApplicationKey { get; set; }

    string GroupName { get; set; }

    string ApplicantName { get; set; }

    int PreviousRequestCount { get; set; }
    bool CurrentYearCalculated { get; set; }

    bool Accepted { get; set; }
    string? RejectionMessage { get; set; }
    bool Blocked { get; set; }
}
