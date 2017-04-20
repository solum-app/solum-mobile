using System.Collections.Generic;
using System.Threading.Tasks;
using Solum.Models;

namespace Solum.Service.Interfaces
{
    public interface IStore<T> where T: EntityData
    {
        Task InsertAsync(T t);
        Task UpdateAsync(T t);
        Task DeleteAsync(T t);
        Task<ICollection<T>> ReadAllAsync();
        Task<T> FindAsync(string id);
    }
}