using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuisinessLogic
{
    public interface CrudRepository<T>
    {
        Task<int> Add(T element);
        Task<List<T>> GetAll();
        Task Update(T element);
        Task Delete(T element);
    }
}
