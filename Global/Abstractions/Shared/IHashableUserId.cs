namespace Global.Abstractions.Global;

public interface IHashableUserId : IHashableUserIdBase
{
    string Id { get; set; }
    bool IdentificationHashed { get; set; }
}
