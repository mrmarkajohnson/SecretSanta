using FluentValidation;
using Global.Abstractions.Areas.Messages;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GlobalSettings;

namespace Application.Areas.Messages.ViewModels;

public class WriteMessageVm : ChooseMessageRecipientVm, IWriteSantaMessage, IOptionalModalFormVm
{
    public WriteMessageVm()
    {
        SubmitButtonText = "Send";
        SubmitButtonIcon = "fa-paper-plane";
        PreviousMessages = new List<ISantaMessage>();
    }

    public bool GroupKeyPreset { get; set; }

    [Display(Name = "Title")]
    public string HeaderText { get; set; } = string.Empty;

    [Display(Name = "Message")]
    public string MessageText { get; set; } = string.Empty;

    public bool Important { get; set; }
    public bool CanReply { get; set; }
    public bool ShowAsFromSanta { get; set; }

    public string AddSuggestionUrl { get; set; } = string.Empty;

    public string PageTitle => IsReply ? "Reply to Message" : "Write a Message";
    public string ModalTitle => PageTitle;
    public string? SubTitle => null;
    public List<string> Guidance => GetGuidance();

    public bool IsModal { get; set; }
    public bool ShowSaveButton => true;
    public string? AdditionalFooterButtonPartial { get; }
    public string GroupWidth => IsModal ? ModalGroupWidth : StandardGroupWidth;

    public int CalendarYear { get; set; } = DateTime.Today.Year;

    public IList<ISantaMessage> PreviousMessages { get; set; }

    private List<string> GetGuidance()
    {
        if (IsReply)
            return [];

        return [$"Send a message to one or more members of {(GiftingGroupKey > 0 ? "the" : "a")} group.",
            $"For gift suggestions, please use the '<a href=\"{AddSuggestionUrl}\">Add Suggestion</a>' option instead of messages."];
    }
}

public class WriteMessageVmValidator : AbstractValidator<WriteMessageVm>
{
    public WriteMessageVmValidator()
    {
        RuleFor(x => x.HeaderText).NotEmpty();
        RuleFor(x => x.MessageText).NotEmpty();

        RuleFor(x => x.GiftingGroupKey)
            .IsInDropDownList(x => x.GroupSelection, false)
            .When(x => x.GroupSelection.Any());

        RuleFor(x => x.GiftingGroupKey)
            .NotNullOrEmpty()
            .When(x => !x.GroupSelection.Any());

        RuleFor(x => x.RecipientType)
            .IsInDropDownList(x => x.AvailableRecipientTypes, false)
            .When(x => x.GiftingGroupKey > 0)
            .WithName("recipient type");

        RuleFor(x => x.SpecificGroupMemberKey)
            .NotNullOrEmpty()
            .When(x => x.RecipientType.SpecificMember());
    }
}
