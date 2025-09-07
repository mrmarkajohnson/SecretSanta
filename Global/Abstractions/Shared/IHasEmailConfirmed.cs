namespace Global.Abstractions.Shared;

public interface IHasEmailConfirmed : IHasEmail
{
    bool EmailConfirmed { get; }
}
