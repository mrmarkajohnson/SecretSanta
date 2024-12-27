namespace Global.Abstractions.Global.Shared;

public interface IUserNamesBase : IUserAllNames, IHashableUserId
{
    string UserDisplayName { get; set; }
}
