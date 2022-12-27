using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IEmployeeRepository : CrudRepository<Employee>
    {
        Task<Employee> GetEmployeeByID(int id);
        Task<List<Employee>> GetResponsibleEmployees(int tid);
        Task<Employee> GetEmployeeByWorkplace(string user, int cid, int? did);
    }
}
