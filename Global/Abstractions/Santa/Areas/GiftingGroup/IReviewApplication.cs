using Global.Abstractions.Global.Shared;

namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IReviewApplication : IHashableUserId, IUserNamesBase
{
    int ApplicationId { get; set; }

    string GroupName { get; set; }

    string ApplicantId { get; set; }
    string IHashableUserId.Id
    {
        get => ApplicantId;
        set => ApplicantId = value;
    }

    string ApplicantName { get; set; }

    int PreviousRequestCount { get; set; }
    bool CurrentYearCalculated { get; set; }

    bool Accepted { get; set; }
    string? RejectionMessage { get; set; }
    bool Blocked { get; set; }
}
