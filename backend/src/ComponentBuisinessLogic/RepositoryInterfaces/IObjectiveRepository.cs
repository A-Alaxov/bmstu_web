using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface IObjectiveRepository : CrudRepository<Objective>
    {
        Task<Objective> GetObjectiveByID(int? id);
        Task<List<Objective>> GetObjectivesByTitle(string title);
        Task<List<Objective>> GetSubObjectives(int tid);
    }
}
