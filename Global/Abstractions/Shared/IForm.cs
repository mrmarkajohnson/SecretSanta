namespace Global.Abstractions.Global;

public interface IForm : IPage
{
    string? ReturnUrl { get; set; }
    string SubmitButtonText { get; set; }
    string SubmitButtonIcon { get; set; }
}
