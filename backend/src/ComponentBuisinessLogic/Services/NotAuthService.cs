using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public class NotAuthService : INotAuthService
    {
        protected readonly IUserRepository UserRepository;
        public NotAuthService(IUserRepository UserRep)
        {
            UserRepository = UserRep;
        }
        public async Task<User> GetUserByLogin(string login)
        {
            return await UserRepository.GetUserByLogin(login);
        }
        public bool AddUser(string login, string password_, string name_, string surname)
        {
            User user = new User(_login: login,
                                 _password_: password_,
                                 _name_: name_,
                                 _surname: surname);
            UserRepository.Add(user);
            return true;
        }
    }
}
