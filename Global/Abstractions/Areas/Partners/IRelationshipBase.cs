namespace Global.Abstractions.Areas.Partners;

public interface IRelationshipBase
{
    Guid GlobalUserId { get; }
    string ManageRelationshipsLink { get; }
}
