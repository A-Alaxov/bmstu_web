using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface INotAuthService
    {
        Task<User> GetUserByLogin(string login);
        bool AddUser(string login, string password_, string name_, string surname);
    }
}
