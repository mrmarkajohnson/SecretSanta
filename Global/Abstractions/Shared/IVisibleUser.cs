namespace Global.Abstractions.Global;

public interface IVisibleUser : IUserNamesBase
{
    IList<string> SharedGroupNames { get; }
}