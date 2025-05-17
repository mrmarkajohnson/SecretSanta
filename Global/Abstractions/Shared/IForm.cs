namespace Global.Abstractions.Shared;

public interface IForm
{
    string? ReturnUrl { get; set; }
    string SubmitButtonText { get; set; }
    string SubmitButtonIcon { get; set; }
}
