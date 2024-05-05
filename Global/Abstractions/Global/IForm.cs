namespace Global.Abstractions.Global;

public interface IForm
{
    string? ReturnUrl { get;}
    string SubmitButtonText { get; }
    string SubmitButtonIcon { get; }
}
