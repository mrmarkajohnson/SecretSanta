using Application.Areas.GiftingGroup.BaseModels;
using FluentValidation;
using Global.Abstractions.Areas.GiftingGroup;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GiftingGroupSettings;
using static Global.Settings.GlobalSettings;

namespace Application.Areas.GiftingGroup.ViewModels;

public class EditGiftingGroupVm : CoreGiftingGroup, IGiftingGroup, IFormVm, IGroupMembersGridVm
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

    public IList<LocationSelectable> Cultures => AvailableCultures
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

    public virtual string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }
    public virtual string SubmitButtonText { get; set; } = "Save";
    public virtual string SubmitButtonIcon { get; set; } = "fa-save";

    Guid? IGroupMembersGridVm.InvitationGuid => null;

    private List<StandardSelectable> GetFirstYearSelection()
    {
        int thisYear = DateTime.Today.Year;

        if (FirstYear > 0 && FirstYear <= thisYear - 2)
            return new List<StandardSelectable> { new(FirstYear, FirstYear.ToString()) };

        if (FirstYear == 0 || FirstYear == thisYear)
        {
            return new List<StandardSelectable>
            {
                new(thisYear, thisYear.ToString()),
                new(thisYear - 1, (thisYear - 1).ToString()),
                new(thisYear - 2, $"{thisYear - 2} or before")
            };
        }
        else
        {
            return new List<StandardSelectable>
            {
                new(thisYear - 1, (thisYear - 1).ToString()),
                new(thisYear - 2, $"{thisYear - 2} or before")
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
