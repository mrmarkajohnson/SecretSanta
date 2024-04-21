using Global.Abstractions.Global;

namespace Global.Abstractions.Santa.Areas.Account;

public interface ISantaUser : IGlobalUser
{
}

public class SantaUserValidator<T> : GlobalUserValidator<T> where T : ISantaUser
{

}