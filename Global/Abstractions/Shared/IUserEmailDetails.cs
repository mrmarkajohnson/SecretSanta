namespace Global.Abstractions.Shared;

public interface IUserEmailDetails : IEmailPreferences, IHasEmail
{
    bool EmailConfirmed { get; }
}
