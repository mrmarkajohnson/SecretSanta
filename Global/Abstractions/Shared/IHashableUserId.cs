namespace Global.Abstractions.Global;

public interface IHashableUserId : IHashableUserIdBase
{
    string GlobalUserId { get; }
    bool IdentificationHashed { get; set; }
}
