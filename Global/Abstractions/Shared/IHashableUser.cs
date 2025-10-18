namespace Global.Abstractions.Shared;

public interface IHashableUser : IHashableUserBase, IHaveAGlobalUserId
{
    bool IdentificationHashed { get; set; }
}
