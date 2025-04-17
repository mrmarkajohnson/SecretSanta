using FluentValidation;
using Global.Abstractions.Areas.Partners;
using ViewLayer.Abstractions;
using static Global.Settings.GlobalSettings;
using static Global.Settings.PartnerSettings;

namespace ViewLayer.Models.Partners;

public sealed class ManageRelationshipVm : RelationshipVm, IRelationship, IModalVm
{
    private YesNoNotSure? _inARelationshipNow;
    private YesNoNotSure? _everInARelationship;
    private YesNoNotSure? _exchangeGifts;

    public RelationshipStatus OriginalStatus { get; set; }

    public YesNoNotSure? InARelationshipNow
    {
        get => _inARelationshipNow ?? Status switch
        {
            RelationshipStatus.ToConfirm => null,
            RelationshipStatus.ToBeConfirmed or RelationshipStatus.Active => YesNoNotSure.Yes,
            _ => YesNoNotSure.No
        };

        set
        {
            _inARelationshipNow = value;
        }
    }

    public YesNoNotSure? EverInARelationship
    {
        get => _everInARelationship ?? Status switch
        {
            RelationshipStatus.ToConfirm => null,
            RelationshipStatus.IgnoreNonRelationship or RelationshipStatus.Avoid => YesNoNotSure.No,
            _ => YesNoNotSure.Yes
        };

        set
        {
            _everInARelationship = value;
        }
    }

    public YesNoNotSure? ExchangeGifts
    {
        get => _exchangeGifts ?? Status switch
        {
            RelationshipStatus.ToConfirm => null,
            RelationshipStatus.IgnoreNonRelationship or RelationshipStatus.IgnoreOld => YesNoNotSure.Yes,
            _ => YesNoNotSure.No
        };

        set
        {
            _exchangeGifts = value;
        }
    }

    public string ModalTitle => (!SuggestedByCurrentUser && AlreadyConfirmed == null ? "Confirm" : "Manage") 
        + (OriginalStatus < RelationshipStatus.Active ? " Suggested" : OriginalStatus == RelationshipStatus.Active ? "" : " Old")
        + " Relationship";

    public bool ShowSaveButton => true;
    public string? SuccessMessage { get; set; }

    public void UpdateStatus()
    {
        Status = _inARelationshipNow switch
        {
            YesNoNotSure.No => _everInARelationship switch
            {
                YesNoNotSure.No => ConfirmationRequiredByOtherPerson()
                    ? RelationshipStatus.IgnoreNonRelationship // essentially cancel the relationship; the suggestor has changed their mind (and 'Avoid' isn't appropriate)
                    : (_exchangeGifts == YesNoNotSure.Yes
                        ? RelationshipStatus.IgnoreNonRelationship :
                        RelationshipStatus.Avoid),
                YesNoNotSure.Yes => _exchangeGifts == YesNoNotSure.Yes
                    ? RelationshipStatus.IgnoreOld
                    : (ConfirmationRequiredByOtherPerson()
                        ? RelationshipStatus.EndedBeforeConfirmation // still need the other person to confirm
                        : RelationshipStatus.Ended),
                _ => OriginalStatus
            },
            YesNoNotSure.Yes => ConfirmationRequiredByOtherPerson()
                ? OriginalStatus // still need the other person to confirm
                : RelationshipStatus.Active,
            _ => OriginalStatus
        };
    }

    private bool ConfirmationRequiredByOtherPerson()
    {
        return OriginalStatus == RelationshipStatus.ToBeConfirmed || (SuggestedByCurrentUser && AlreadyConfirmed == null);
    }
}

public sealed class ManageRelationshipVmValidator : AbstractValidator<ManageRelationshipVm>
{
    public ManageRelationshipVmValidator()
    {
        RuleFor(x => x.InARelationshipNow)
            .NotEmpty();

        RuleFor(x => x.EverInARelationship)
            .NotEmpty()
            .When(x => x.InARelationshipNow != YesNoNotSure.Yes && x.OriginalStatus <= RelationshipStatus.ToConfirm);

        RuleFor(x => x.ExchangeGifts)
            .NotEmpty()
            .When(x => x.InARelationshipNow != YesNoNotSure.Yes);

        RuleFor(x => x.ExchangeGifts)
            .NotEqual(YesNoNotSure.NotSure)
            .When(x => x.InARelationshipNow != YesNoNotSure.Yes)
            .WithMessage(x => $"Please talk to {x.Partner.UserDisplayName} to agree what you'd like to do.");
    }
}
