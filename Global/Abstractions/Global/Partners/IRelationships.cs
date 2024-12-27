namespace Global.Abstractions.Global.Partners;

public interface IRelationships
{
    IList<IRelationship> PossibleRelationships { get; }
}
