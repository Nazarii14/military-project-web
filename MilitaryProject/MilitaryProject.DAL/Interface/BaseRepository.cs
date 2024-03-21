using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitaryProject.DAL.Interface
{
    public interface BaseRepository<T>
    {
        Task Create(T entity);
        Task Delete (T entity);
        Task<IEnumerable<T>> GetAll();
        Task Update (T entity);
    }
}
