using FluentValidation;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.GiftingGroup;

public class JoinGiftingGroupVm : BaseFormVm, IJoinGiftingGroup
{
    public int? GroupId { get; set; }

    [Required, Display(Name = "Group Name")]
    public string Name { get; set; } = "";

    [Display(Name = "Joiner Token"), Required]
    public string JoinerToken { get; set; } = "";

    [Required, StringLength(GiftingGroupVal.Description.MaxLength, MinimumLength = GiftingGroupVal.Description.MinLength)]
    public string Description { get; set; } = "";

    [MaxLength(GiftingGroupVal.JoinerMessage.MaxLength)]
    public string? Message { get; set; }

    public bool Blocked { get; set; }
    public bool AlreadyMember { get; set; }
    public bool ApplicationPending { get; set; }

    public required string GetGroupDetailsAction { get; set; } = "GetGroupDetailsForJoiner";

    public override string SubmitButtonText { get; set; } = "Join";
    public override string SubmitButtonIcon { get; set; } = "fa-handshake";

    public class JoinGiftingGroupVmValidator : AbstractValidator<JoinGiftingGroupVm>
    {
        public JoinGiftingGroupVmValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.JoinerToken).NotEmpty();
            RuleFor(x => x.Message).MaximumLength(GiftingGroupVal.JoinerMessage.MaxLength);
        }
    }
}
