using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IFounderService : IManagerService
    {
        Task<EmployeeView> AddEmployee(Employee empl, string user_, int permission_, int? department = -1);
        Task<bool> UpdateEmployee(Employee empl, int id, string user_, int permission_, int? department = -1);
        Task<bool> DeleteEmployee(Employee empl, int id);
        Task<WorkplaceView> UpdateCompany(Employee empl, string title, int foundationyear);
        Task<bool> DeleteCompany(Employee empl);
    }
}
