namespace Global.Abstractions.Santa.Areas.Account;

public interface IUpdateSantaUser : ISantaUser
{
    string Password { get; set; }
}


