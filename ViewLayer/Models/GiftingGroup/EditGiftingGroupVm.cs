using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.System;
using static Global.Settings.GlobalSettings;

using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Global.Validation;

namespace ViewLayer.Models.GiftingGroup;

public class EditGiftingGroupVm : CoreGiftingGroup, IGiftingGroup, IForm
{
    public bool Exists => Id > 0;

    [Display(Name = "Currency")]
    public string CurrencyOverride
    {
        get => CultureInfoExtensions.GetCurrencyString(CurrencyCodeOverride, CurrencySymbolOverride);
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                CurrencyCodeOverride = CurrencySymbolOverride = "";
            }
            else if (value.Contains(" ("))
            {
                string[] parts = value.Split(" (");
                CurrencyCodeOverride = parts[0];
                CurrencySymbolOverride = parts[1].Replace(")", "");
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
        .Where(x => !string.IsNullOrWhiteSpace(x.CurrencyString))
        .DistinctBy(x => x.CurrencyString)
        .OrderBy(x => x.CurrencyString)
        .ToList();

    public virtual string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }
    public virtual string SubmitButtonText { get; set; } = "Save";
    public virtual string SubmitButtonIcon { get; set; } = "fa-save";
}

public class EditGiftingGroupVmValidator : AbstractValidator<EditGiftingGroupVm>
{
    public EditGiftingGroupVmValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(GiftingGroupVal.Name.MinLength, GiftingGroupVal.Name.MaxLength);
        RuleFor(x => x.Description).NotEmpty().Length(GiftingGroupVal.Description.MinLength, GiftingGroupVal.Description.MaxLength);
        RuleFor(x => x.JoinerToken).NotEmpty();
        RuleFor(x => x.CultureInfo).IsInDropDownList(x => x.Cultures.Select(y => y.Name), false);
        RuleFor(x => x.CurrencyOverride).IsInDropDownList(x => x.Currencies.Select(y => y.Name), true);
    }
}
