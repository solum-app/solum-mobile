//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Solum.Models;
//using Solum.Service.Interfaces;

//namespace Solum.Service
//{
//    public class TalhaoService : AzureService<Talhao>, IStore<Talhao>
//    {
//        private static TalhaoService _instance;

//        public static TalhaoService Instance => _instance ?? (_instance = new TalhaoService());

//        public void Add(Talhao t)
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task UpdateAsync(Talhao t)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void Delete(Talhao t)
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task<IList<Talhao>> ReadAllAsync()
//        {
//            throw new System.NotImplementedException();
//        }

//        public Talhao Find(string id)
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task<bool> HasAnalises(string talhaoid)
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task<IList<Talhao>> FromFazenda(string fazendaId)
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task<IList<Talhao>> FromFazendaAsync(string fazendaId)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}