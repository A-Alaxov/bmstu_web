using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IResponsibleService : IEmployeeService
    {
        Task<int> AddSubObjective(Employee empl, int pid, string title, DateTime termBegin, DateTime termEnd, TimeSpan estimatedTime);
        Task<bool> UpdateObjective(Employee empl, int tid, string title, DateTime termBegin, DateTime termEnd, TimeSpan estimatedTime);
        Task<bool> DeleteSubObjective(Employee empl, int id);
        Task<List<EmployeeView>> GetResponsibleEmployees(Employee empl, int id);
        Task<bool> AddResponsibility(Employee empl, int eid, int tid, TimeSpan timeAmount);
        Task<bool> DeleteResponsibility(Employee empl, int rid);
    }
}
