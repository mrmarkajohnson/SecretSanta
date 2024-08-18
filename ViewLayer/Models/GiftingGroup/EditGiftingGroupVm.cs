using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.System;
using static Global.Settings.GlobalSettings;

using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.GiftingGroup;

public class EditGiftingGroupVm : CoreGiftingGroup, IGiftingGroup, IForm
{
    public bool Exists => Id > 0;

    [Display(Name = "Currency")]
    public string CurrencyOverride { get; set; } = "";

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
