using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IManagerService : IEmployeeService
    {
        Task<int> AddObjective(Employee empl, int? pid, string title, DateTime termBegin, DateTime termEnd, TimeSpan estimatedTime, int? department = -1);
        Task<bool> UpdateObjective(Employee empl, int tid, string title, DateTime termBegin, DateTime termEnd, TimeSpan estimatedTime, int? department = -1);
        Task<bool> DeleteObjective(Employee empl, int id);
        Task<bool> AddResponsible(Employee empl, int eid, int tid);
        Task<bool> DeleteResponsibility(Employee empl, int rid);
        Task<List<ResponsibilityView>> GetResponsibilityByEmployee(Employee empl, int id);
        Task<List<EmployeeView>> GetResponsibleEmployees(Employee empl, int id);
        Task<int> AddDepartment(Employee empl, string title, int foundationyear, string activityfield);
        bool UpdateDepartment(Employee empl, int id, string title, int foundationyear, string activityfield);
        Task<bool> DeleteDepartment(Employee empl, int id);
        bool CheckEmployeeDepartment(Employee empl, int id);
    }
}
