namespace Global.Abstractions.Global.Shared;

public interface IVisibleUser : IUserNamesBase
{
    IList<string> SharedGroupNames { get; }
}