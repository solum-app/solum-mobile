using System.Collections.Generic;
using System.Linq;
using Realms;

namespace Solum.Service
{
    public class DataService<T> where T : RealmObject
    {
        protected Realm Instance { get; }

        public DataService()
        {
            Instance = Realm.GetInstance();
        }

        public bool Add(T t)
        {
            using (var transaction = Instance.BeginWrite())
            {
                try
                {
                    Instance.Add(t, true);
                    transaction.Commit();
                    return true;
                }
                catch (RealmException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public bool Update(T t)
        {
            using (var transaction = Instance.BeginWrite())
            {
                try
                {
                    Instance.Add(t, true);
                    transaction.Commit();
                    return true;
                }
                catch (RealmException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public T Find(string id)
        {
            return Instance.Find<T>(id);
        }

        public IList<T> Find()
        {
            return Instance.All<T>().ToList();
        }

        public bool Delete(T t)
        {
            using (var transaction = Instance.BeginWrite())
            {
                try
                {
                    Instance.Remove(t);
                    transaction.Commit();
                    return true;
                }
                catch (RealmException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public bool Delete(string id)
        {
            using (var transaction = Instance.BeginWrite())
            {
                try
                {
                    var found = Find(id);
                    Instance.Remove(found);
                    transaction.Commit();
                    return true;
                }
                catch (RealmException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}