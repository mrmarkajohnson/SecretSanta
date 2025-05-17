namespace Global.Abstractions.Shared;

public interface IHashableUser : IHashableUserBase, IHasGlobalUserId
{
    bool IdentificationHashed { get; set; }
}
