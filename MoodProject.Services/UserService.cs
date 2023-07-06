using MoodProject.Core;
using MoodProject.Core.Ports.In;

namespace MoodProject.Services;

public class UserService : IUserService
{
    private HttpClient ApiClient;
    public UserService(HttpClient apiClient)
    {
        ApiClient = apiClient;
    }
    public User GetUserData(int id)
    {
        throw new NotImplementedException();
    }

    public User Register(User user)
    {
        throw new NotImplementedException();
    }

    public bool Login(Credentials credentials)
    {
        throw new NotImplementedException();
    }
}