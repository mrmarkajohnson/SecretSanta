using System.ComponentModel.DataAnnotations;

namespace Global.Settings;

public static class GiftingGroupSettings
{
    public enum YearCalculationOption
    {
        [Display(Name = "Keep existing")]
        None = 0,

        [Display(Name = "Assign now")]
        Calculate = 1,

        [Display(Name = "Cancel and reset")]
        Cancel = 2
    }

    public const int MinimumParticipatingMembers = 3;

    public enum GroupMemberStatus
    {
        [Display(Name = "Not Applied")]
        None = 0,

        [Display(Name = "Applied to Join")]
        Applied,

        [Display(Name = "Application Rejected")]
        Rejected,

        [Display(Name = "Member")]
        Joined,

        [Display(Name = "Administrator")]
        Admin
    }

    public enum OtherGroupMembersType
    {
        EditGroup,
        ReviewInvitation,
        MessageRecipients
    }
}
