namespace Global.Abstractions.Santa.Areas.GiftingGroup;

public interface IReviewApplication
{
    int ApplicationId { get; set; }

    string GroupName { get; set; }

    string UserName { get; set; }
    string Name { get; set; }
    string? EmailAddress { get; set; }

    int PreviousRequestCount { get; set; }

    bool Accepted { get; set; }
    string? RejectionMessage { get; set; }
    bool Blocked { get; set; }
}
