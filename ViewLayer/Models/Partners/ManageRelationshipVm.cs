using FluentValidation;
using Global.Abstractions.Areas.Partners;
using ViewLayer.Abstractions;
using static Global.Settings.GlobalSettings;
using static Global.Settings.PartnerSettings;

namespace ViewLayer.Models.Partners;

public class ManageRelationshipVm : RelationshipVm, IRelationship, IModalVm
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
            UpdateStatus();
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
            UpdateStatus();
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
            UpdateStatus();
        }
    }

    public string ModalTitle => OriginalStatus == RelationshipStatus.ToConfirm ? "Confirm Relationship" : "Manage Relationship";
    public bool ShowSaveButton => true;

    private void UpdateStatus()
    {
        Status = _inARelationshipNow switch
        {
            YesNoNotSure.No => _everInARelationship switch
            {
                YesNoNotSure.No => OriginalStatus == RelationshipStatus.ToBeConfirmed
                    ? RelationshipStatus.EndedBeforeConfirmation
                    : (_exchangeGifts == YesNoNotSure.Yes ? RelationshipStatus.IgnoreNonRelationship : RelationshipStatus.Avoid),
                YesNoNotSure.Yes => OriginalStatus == RelationshipStatus.ToBeConfirmed
                    ? RelationshipStatus.ToBeConfirmed
                    : (_exchangeGifts == YesNoNotSure.Yes ? RelationshipStatus.IgnoreOld : RelationshipStatus.Ended),
                _ => OriginalStatus
            },
            YesNoNotSure.Yes => OriginalStatus == RelationshipStatus.ToBeConfirmed
                    ? RelationshipStatus.ToBeConfirmed
                    : RelationshipStatus.Active,
            _ => OriginalStatus
        };
    }
}

public class ManageRelationshipVmValidator : AbstractValidator<ManageRelationshipVm>
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
