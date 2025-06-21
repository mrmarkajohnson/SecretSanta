using Global.Abstractions.Shared;

namespace Global.Abstractions.Areas.Account;

public interface ISantaUser : IGlobalUser
{
    int? SantaUserKey { get; }
}

public class SantaUserValidator<T> : GlobalUserValidator<T> where T : ISantaUser
{
}