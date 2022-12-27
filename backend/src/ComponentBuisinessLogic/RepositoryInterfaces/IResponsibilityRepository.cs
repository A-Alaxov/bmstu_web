using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IResponsibilityRepository : CrudRepository<Responsibility>
    {
        Task<Responsibility> GetResponsibilityByID(int rid);
        Task<List<Responsibility>> GetResponsibilityByEmployee(int eid);
        Task<Responsibility> GetResponsibilityByObjectiveAndEmployee(int tid, int eid);
    }
}
