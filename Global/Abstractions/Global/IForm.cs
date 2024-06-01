namespace Global.Abstractions.Global;

public interface IForm : IPage
{
    string? ReturnUrl { get;}
    string SubmitButtonText { get; }
    string SubmitButtonIcon { get; }
}
