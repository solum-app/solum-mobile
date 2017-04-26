//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Solum.Models;
//using Solum.Service.Interfaces;

//namespace Solum.Service
//{
//    public class AnaliseService : AzureService<Analise>, IStore<Analise>
//    {
//        private static AnaliseService _instance;

//        private AnaliseService()
//        {
//            Table = Client.GetSyncTable<Analise>();
//        }

//        public static AnaliseService Instance => _instance ?? (_instance = new AnaliseService());

//        public void Add(Analise t)
//        {
//            Table.InsertAsync(t);
//        }

//        public void Update(Analise t)
//        {
//            throw new NotImplementedException();
//        }

//        public void Delete(Analise t)
//        {
//            await Table.DeleteAsync(t);
//        }

//        public IList<Analise> ReadAll()
//        {
//            var queryable = Table.CreateQuery();
//        }

//        public ICollection<Analise> All()
//        {
//            throw new NotImplementedException();
//        }

//        public Analise Find(string id)
//        {
//            var result = await Table.LookupAsync(id);
//            return result;
//        }

//        public bool ExistsFromTalhaoAsync(string talhaoId)
//        {
//            var query = Table.CreateQuery();
//            var list = await query.Where(c => c.TalhaoId.Equals(talhaoId)).ToListAsync();
//            return list.Any();
//        }
//    }
//}