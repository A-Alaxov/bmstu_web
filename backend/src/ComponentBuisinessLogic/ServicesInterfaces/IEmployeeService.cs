using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IEmployeeService : IUserService
    {
        Task<List<Department>> GetAllDepartments(Employee empl);
        Task<List<EmployeeView>> GetAllEmployees(Employee empl);
        Task<List<Objective>> GetAllObjectives(Employee empl);
        Task<List<Objective>> GetAllObjectivesTest(Employee empl);
        Task<List<Objective>> GetObjectiveByID(Employee empl, int tid);
        Task<List<Objective>> GetObjectivesByTitle(Employee empl, string title);
        Task<WorkplaceView> GetWorkplace(Employee empl);
    }
}
