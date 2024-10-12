namespace Global.Abstractions.Global.Shared;

public interface IHashableUserId : IHashableUserIdBase
{
    string Id { get; set; }    
    bool IdentificationHashed { get; set; }
}
