namespace MoodProject.Core.Ports.In;

public interface IUserService
{
    public User GetUserData(int id);
    public User Register(User user);
    public bool Login(Credentials credentials);
}