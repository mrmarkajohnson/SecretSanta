namespace Global.Abstractions.Shared;

public interface IVisibleUser : IUserNamesBase
{
    IList<string> SharedGroupNames { get; }
}