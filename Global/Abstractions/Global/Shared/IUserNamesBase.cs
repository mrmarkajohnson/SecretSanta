namespace Global.Abstractions.Global.Shared;

public interface IUserNamesBase : IUserAllNames
{
    string UserDisplayName { get; set; }
}
