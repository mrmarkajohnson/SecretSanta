using Global.Abstractions.Global.Account;

namespace Global.Abstractions.Santa.Areas.Account;

public interface IRegisterSantaUser : ISantaUser, ISetPassword
{
}

public class RegisterSantaUserValidator<T> : SantaUserValidator<T> where T : IRegisterSantaUser
{
	public RegisterSantaUserValidator()
	{
		Include(new SetPasswordValidator<T>());
	}
}