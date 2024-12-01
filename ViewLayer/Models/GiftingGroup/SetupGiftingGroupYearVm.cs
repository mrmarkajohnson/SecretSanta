using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using static Global.Settings.GiftingGroupSettings;

namespace ViewLayer.Models.GiftingGroup;

public class SetupGiftingGroupYearVm : GiftingGroupYear, IGiftingGroupYear, IForm
{
    public bool Calculate
    {
        get => CalculationOption == YearCalculationOption.Calculate;
        set
        {
            if (value)
            {
                CalculationOption = YearCalculationOption.Calculate;
            }
            else if (CalculationOption == YearCalculationOption.Calculate) // otherwise leave as 'Cancel'
            {
                CalculationOption = YearCalculationOption.None;
            }
        }
    }
    
    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa-save";
    public string? SuccessMessage { get; set; }
}

public class SetupGiftingGroupYearVmValidator : GiftingGroupYearValidator<SetupGiftingGroupYearVm>
{
}
