using FluentValidation;
using Global.Extensions.System;
using static Global.Settings.GiftingGroupSettings;

namespace Global.Abstractions.Areas.GiftingGroup;

public interface IGiftingGroupYear : IGiftingGroupYearBase
{
    string GiftingGroupName { get; }
    string CurrencyCode { get; set; }
    string CurrencySymbol { get; set; }

    bool Calculated { get; set; }
    bool RecalculationRequired { get; set; }
    YearCalculationOption CalculationOption { get; set; }

    IList<IYearGroupUserBase> GroupMembers { get; }
    string? PreviousYearsWarning { get; set; }
}

public class GiftingGroupYearValidator<TItem> : AbstractValidator<TItem> where TItem : IGiftingGroupYear
{
    public GiftingGroupYearValidator()
    {
        RuleFor(x => x.Limit)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.CalculationOption)
            .NotEqual(YearCalculationOption.Calculate)
            .When(x => x.GroupMembers.Count(m => m.Included == true) < MinimumParticipatingMembers)
            .WithMessage("There are not enough participating members to assign givers and receivers.");

        RuleFor(x => x.CalculationOption)
            .NotEqual(YearCalculationOption.Calculate)
            .When(x => x.GroupMembers.Any(m => m.Included == null))
            .WithMessage("Cannot assign givers and receivers; some members have not been set as participating or not.");

        RuleFor(x => x.CalculationOption)
            .NotEqual(YearCalculationOption.None)
            .When(x => x.RecalculationRequired)
            .WithMessage(x => "The current assigned givers and receivers no longer cover the participating members. " +
                (x.GroupMembers.Count(m => m.Included == true) < MinimumParticipatingMembers
                    ? $"There are not enough participating members to reassign, " +
                        $"so please select the '{YearCalculationOption.Cancel.DisplayName()}' option."
                    : "Please either reassign or cancel givers and receivers."));
    }
}
