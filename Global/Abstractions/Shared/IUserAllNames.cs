namespace Global.Abstractions.Global;

public interface IUserAllNames
{
    string Forename { get; set; }
    string? MiddleNames { get; set; }
    string Surname { get; set; }
}
