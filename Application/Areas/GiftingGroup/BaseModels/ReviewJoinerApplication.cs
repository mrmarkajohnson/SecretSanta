using Application.Shared.BaseModels;
using Global.Abstractions.Areas.GiftingGroup;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.GiftingGroup.BaseModels;

public class ReviewJoinerApplication : UserNamesBase, IReviewApplication
{
    public int GroupApplicationKey { get; set; }

    [Display(Name = "Group Name")]
    public string GroupName { get; set; } = string.Empty;

    [Display(Name = $"Applicant {UserDisplayNames.UserName}")]
    public override string? UserName { get; set; } = string.Empty;

    [Display(Name = "Applicant Name")]
    public string ApplicantName
    {
        get => UserDisplayName;
        set => UserDisplayName = value;
    }

    [Display(Name = $"Applicant {UserDisplayNames.Email}")]
    public override string? Email { get; set; }

    public int PreviousRequestCount { get; set; }
    public bool CurrentYearCalculated { get; set; }

    public bool Accepted { get; set; }

    [Display(Name = "Message")]
    public string? RejectionMessage { get; set; }

    [Display(Name = "Block Future Applications")]
    public bool Blocked { get; set; }
}
