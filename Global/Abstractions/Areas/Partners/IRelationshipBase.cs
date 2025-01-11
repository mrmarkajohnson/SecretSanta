namespace Global.Abstractions.Areas.Partners;

public interface IRelationshipBase
{
    Guid UserId { get; }
    string ManageRelationshipsLink { get; }
}
