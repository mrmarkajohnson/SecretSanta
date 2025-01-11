namespace Global.Abstractions.Global;

public interface IUserNamesBase : IUserAllNames, IHashableUserId
{
    string UserDisplayName { get; set; }
}
