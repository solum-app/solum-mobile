using System.Collections.Generic;
using Realms;

namespace Solum.Service
{
    public class DataService<T>
    {
        public Realm Instance { get;}

        public DataService()
        {
            Instance = Realm.GetInstance();
        }

        public bool Add(T t)
        {
            return false;
        }

        public bool Update(T t)
        {
            return false;
        }

        public T Find(string id)
        {
            return default(T);
        }

        public IList<T> Find()
        {
            return null;
        }

        public bool Delete(T t)
        {
            return false;
        }

        public bool Delete(string id)
        {
            return false;
        }
    }
}