namespace Global.Abstractions.Areas.Partners;

public interface IRelationships
{
    IList<IRelationship> PossibleRelationships { get; }
}
