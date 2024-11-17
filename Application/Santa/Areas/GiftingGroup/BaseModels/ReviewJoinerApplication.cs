using Global.Abstractions.Santa.Areas.GiftingGroup;
using System.ComponentModel.DataAnnotations;

namespace Application.Santa.Areas.GiftingGroup.BaseModels;

public class ReviewJoinerApplication : UserNamesBase, IReviewApplication
{
    public int ApplicationId { get; set; }

    public string ApplicantId { get; set; } = string.Empty;

    [Display(Name = "Group Name")]
    public string GroupName { get; set; } = string.Empty;

    [Display(Name = "Applicant UserName")]
    public string? UserName { get; set; } = string.Empty;

    [Display(Name = "Applicant Name")]
    public string ApplicantName
    {
        get => UserDisplayName;
        set => UserDisplayName = value;
    }

    [Display(Name = "Applicant E-mail Address")]
    public string? Email { get; set; }

    public int PreviousRequestCount { get; set; }

    public bool Accepted { get; set; }

    [Display(Name = "Message")]
    public string? RejectionMessage { get; set; }

    [Display(Name = "Block Future Applications")]
    public bool Blocked { get; set; }

    public bool IdentificationHashed { get; set; }
}
