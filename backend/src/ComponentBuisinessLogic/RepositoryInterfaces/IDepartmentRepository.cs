using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IDepartmentRepository : CrudRepository<Department>
    {
        Task<Department> GetDepartmentByID(int? departmentid);
        Task<List<Department>> GetDepartmentsByCompany(int company);
    }
}
