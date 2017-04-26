using System.Collections.Generic;
using Solum.Models;

namespace Solum.Service.Interfaces
{
    public interface IStore<T> where T : EntityData
    {
        void Add(T t);
        void Update(T t);
        void Delete(T t);
        IList<T> ReadAll();
        ICollection<T> All();
        T Find(string id);

    }
}