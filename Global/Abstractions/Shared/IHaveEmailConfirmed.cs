namespace Global.Abstractions.Shared;

public interface IHaveEmailConfirmed : IHaveAnEmail
{
    bool EmailConfirmed { get; }
}
