using static Global.Settings.MessageSettings;

namespace Application.Areas.Messages.ViewModels;

public sealed class ReportIssueVm : WriteMessageVm
{
	public ReportIssueVm()
	{
        GroupKeyPreset = true;
        RecipientType = MessageRecipientType.SystemAdmins;
    }

    public override string PageTitle => "Report an Issue";
    protected override List<string> GetGuidance()
    {
        return ["Use this form to advise system administrators of a problem with the system.",
            $"Please add any relevant details you can think of, including steps to replicate the issue."];
    }

    public override IList<StandardSelectable> GroupSelection => new List<StandardSelectable>();
    public override List<MessageRecipientType> AvailableRecipientTypes => [MessageRecipientType.SystemAdmins];
    public override IList<StandardSelectable> MemberSelection => new List<StandardSelectable>();
}
