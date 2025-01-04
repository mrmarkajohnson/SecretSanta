namespace Global.Abstractions.Global.Partners;

public interface IRelationshipBase
{
    Guid UserId { get; }
    string ManageRelationshipsLink { get; }
}
