//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Solum.Handlers;
//using Solum.Models;
//using Solum.Service.Interfaces;

//namespace Solum.Service
//{
//    public class FazendaService : AzureService<Fazenda>, IStore<Fazenda>
//    {
//        private static FazendaService _instance;

//        public static FazendaService Instance => _instance ?? (_instance = new FazendaService());

//        private FazendaService()
//        {
//            Table = Client.GetSyncTable<Fazenda>();
//        }

//        public async void Add(Fazenda t)
//        {
//            await Table.InsertAsync(t);
//        }

//        public async Task UpdateAsync(Fazenda t)
//        {
//            await Table.UpdateAsync(t);
//        }

//        public async void Delete(Fazenda t)
//        {
//            await Table.DeleteAsync(t);
//        }

//        public async Task<IList<Fazenda>> ReadAllAsync()
//        {
//            var fazendas = await Table.ReadAsync();
//            var queryable = fazendas.AsQueryable();
//            var userFazendas = await queryable.PerUser();
//            return userFazendas.ToList();
//        }

//        public async Fazenda Find(string id)
//        {
//            return await Table.LookupAsync(id);
//        }
//    }
//}