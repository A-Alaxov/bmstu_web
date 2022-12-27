using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IHRService : IEmployeeService
    {
        Task<List<EmployeeView>> GetResponsibleEmployees(Employee empl, int id);
        Task<EmployeeView> AddEmployee(User usr, Employee empl, string user_, int permission_, int? department = -1);
        Task<bool> UpdateEmployee(Employee empl, int id, string user_, int permission_, int? department = -1);
        Task<bool> DeleteEmployee(Employee empl, int id);
    }
}
