using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IUserService
    {
        UserView GetUser(User usr);
        Task<WorkplaceView> AddCompany(User usr, string title, int foundationyear);
        Task<List<WorkplaceView>> GetWorkplaces(User usr);
        Task<Employee> GetEmployeeByWorkplace(User usr, int id);
        Task<UserView> UpdateUser(User usr, string name_, string surname);
        Task<bool> DeleteUser(User usr);
    }
}
