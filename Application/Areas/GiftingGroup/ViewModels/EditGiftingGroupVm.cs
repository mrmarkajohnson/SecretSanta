using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.GiftingGroup.ViewModels;

public sealed class EditGiftingGroupVm : BaseModels.CoreGiftingGroup, IGiftingGroup, IFormVm, IGroupMembersGridVm
{
    public OtherGroupMembersType MemberListType => OtherGroupMembersType.EditGroup;
    public bool Exists => GiftingGroupKey > 0;

    [Display(Name = "Currency")]
    public string CurrencyOverride
    {
        get => CultureInfoExtensions.GetCurrencyString(CurrencyCodeOverride, CurrencySymbolOverride);
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                CurrencyCodeOverride = CurrencySymbolOverride = string.Empty;
            }
            else if (value.Contains(" ("))
            {
                string[] parts = value.Split(" (");
                CurrencyCodeOverride = parts[0];
                CurrencySymbolOverride = parts[1].Replace(")", string.Empty);
            }
            else
            {
                CurrencyCodeOverride = CurrencySymbolOverride = value;
            }
        }
    }

    public string? DefaultCurrency => Cultures?.FirstOrDefault(x => x.Name == CultureInfo)?.CurrencyString;

    public IList<LocationSelectable> Cultures => GlobalSettings.AvailableCultures
        .Select(x => x.CultureLocation())
        .OrderBy(x => x.Location)
        .ToList();

    public IList<LocationSelectable> Currencies => Cultures
        .Where(x => x.CurrencyString.IsNotEmpty())
        .DistinctBy(x => x.CurrencyString)
        .OrderBy(x => x.CurrencyString)
        .ToList();

    public IList<StandardSelectable> FirstYears => GetFirstYearSelection();
    public IEnumerable<IGroupMember> OtherGroupMembers { get; set; } = new List<IGroupMember>();

    public string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";

    Guid? IGroupMembersGridVm.InvitationGuid => null;

    private List<StandardSelectable> GetFirstYearSelection()
    {
        int currentYear = FirstYear <= 0 || FirstYear > GlobalSettings.CurrentYear ? DateTime.Today.Year : GlobalSettings.CurrentYear;

        if (FirstYear > 0 && FirstYear <= currentYear - 2)
            return new List<StandardSelectable> { new(FirstYear, FirstYear.ToString()) };

        if (FirstYear <= 0 || FirstYear == currentYear)
        {
            return new List<StandardSelectable>
            {
                new(currentYear, currentYear.ToString()),
                new(currentYear - 1, (currentYear - 1).ToString()),
                new(currentYear - 2, $"{currentYear - 2} or before")
            };
        }
        else
        {
            return new List<StandardSelectable>
            {
                new(currentYear - 1, (currentYear - 1).ToString()),
                new(currentYear - 2, $"{currentYear - 2} or before")
            };
        }
    }
}

public sealed class EditGiftingGroupVmValidator : AbstractValidator<EditGiftingGroupVm>
{
    public EditGiftingGroupVmValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(GiftingGroupVal.Name.MinLength, GiftingGroupVal.Name.MaxLength);
        RuleFor(x => x.Description).NotEmpty().Length(GiftingGroupVal.Description.MinLength, GiftingGroupVal.Description.MaxLength);
        RuleFor(x => x.JoinerToken).NotEmpty();
        RuleFor(x => x.CultureInfo).IsInDropDownList(x => x.Cultures.Select(y => y.Name), false);
        RuleFor(x => x.CurrencyOverride).IsInDropDownList(x => x.Currencies
            .Where(x => x.CurrencyString != null)
            .Select(y => y.CurrencyString ?? string.Empty), true);
    }
}
