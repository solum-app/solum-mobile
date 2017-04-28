using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Plugin.Connectivity;
using Solum.Handlers;
using Solum.Helpers;
using Solum.Models;

namespace Solum.Service
{
    public class AzureService
    {
        private static AzureService _instance;

        public static AzureService Instance = _instance ?? (_instance = new AzureService());
        private readonly MobileServiceClient _client;

        private AzureService()
        {
            if (_client == null) _client = new MobileServiceClient(Settings.BaseUri);
        }

        private async Task Initialize()
        {
            if (_client?.SyncContext?.IsInitialized ?? false)
                return;

            var store = new MobileServiceSQLiteStore(Settings.DBPath);
            store.DefineTable<Estado>();
            store.DefineTable<Cidade>();
            store.DefineTable<Fazenda>();
            store.DefineTable<Talhao>();
            store.DefineTable<Analise>();
            var task = _client?.SyncContext.InitializeAsync(store);
            if (task != null) await task;
        }

        #region Sincronizacao

        public async Task Sync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await PullEstados();
                await PullCidades();
                await PullFazendas();
                await PullTalhoes();
                await PullAnalises();
                await _client.SyncContext.PushAsync();
            }
        }

        private async Task PullEstados()
        {
            await Initialize();
            string queryName = $"incsync_{typeof(Estado).Name}";
            var table = _client.GetSyncTable<Estado>();
            await table.PullAsync(queryName, table.CreateQuery());
        }

        private async Task PullCidades()
        {
            await Initialize();
            var table = _client.GetSyncTable<Cidade>();
            string queryName = $"incsync_{typeof(Cidade).Name}";
            await table.PullAsync(queryName, table.CreateQuery());
        }

        private async Task PullFazendas()
        {
            await Initialize();
            string queryName = $"incsync_{typeof(Fazenda).Name}";
            var table = _client.GetSyncTable<Fazenda>();
            await table.PullAsync(queryName, table.CreateQuery());
        }

        private async Task PullTalhoes()
        {
            await Initialize();
            string queryName = $"incsync_{typeof(Talhao).Name}";
            var table = _client.GetSyncTable<Talhao>();
            await table.PullAsync(queryName, table.CreateQuery());
        }

        private async Task PullAnalises()
        {
            await Initialize();
            string queryName = $"incsync_{typeof(Analise).Name}";
            var table = _client.GetSyncTable<Analise>();
            await table.PullAsync(queryName, table.CreateQuery());
        }

        #endregion

        #region Metodos para Estado

        public async Task<IList<Estado>> ListEstadosAsync()
        {
            await Initialize();
            var table = _client.GetSyncTable<Estado>();
            var query = table.OrderBy(e => e.Nome);
            return await query.ToListAsync();
        }

        #endregion

        #region Metodos para Cidade

        public async Task<IList<Cidade>> ListCidadesAsync(string estadoId)
        {
            await Initialize();
            var table = _client.GetSyncTable<Cidade>();
            var query = table.CreateQuery();
            query = query.Where(c => c.EstadoId == estadoId).OrderBy(c => c.Nome);
            return await query.ToListAsync();
        }

        public async Task<Cidade> FindCidadeAsync(string cidadeId)
        {
            await Initialize();
            var table = _client.GetSyncTable<Cidade>();
            return await table.LookupAsync(cidadeId);
        }

        #endregion

        #region Metodos para fazenda

        public async Task InsertFazendaAsync(Fazenda fazenda)
        {
            await Initialize();
            var table = _client.GetSyncTable<Fazenda>();
            await table.InsertAsync(fazenda);
        }

        public async Task UpdateFazendaAsync(Fazenda fazenda)
        {
            await Initialize();
            var table = _client.GetSyncTable<Fazenda>();
            await table.UpdateAsync(fazenda);
        }

        public async Task DeleteFazendaAsync(Fazenda fazenda)
        {
            await Initialize();
            var table = _client.GetSyncTable<Fazenda>();
            await table.DeleteAsync(fazenda);
        }

        public async Task<Fazenda> FindFazendaAsync(string fazendaid)
        {
            await Initialize();
            var table = _client.GetSyncTable<Fazenda>();
            return await table.LookupAsync(fazendaid);
        }

        public async Task<IList<Fazenda>> ListFazendaAsync()
        {
            await Initialize();
            var estadoTable = _client.GetSyncTable<Estado>();
            var cidadeTable = _client.GetSyncTable<Cidade>();
            var fazendaTable = _client.GetSyncTable<Fazenda>();
            var fazendas = await fazendaTable.CreateQuery().OrderBy(f => f.Nome).ToListAsync();
            foreach (var fazenda in fazendas)
            {
                var cidade = await cidadeTable.LookupAsync(fazenda.CidadeId);
                cidade.Estado = await estadoTable.LookupAsync(cidade.EstadoId);
                fazenda.Cidade = cidade;
            }
            return fazendas;
        }

        #endregion


        #region Metodos para Talhao

        public async Task InsertTalhaoAsync(Talhao talhao)
        {
            await Initialize();
            var table = _client.GetSyncTable<Talhao>();
            await table.InsertAsync(talhao);
        }

        public async Task UpdateTalhaoAsync(Talhao talhao)
        {
            await Initialize();
            var table = _client.GetSyncTable<Talhao>();
            await table.UpdateAsync(talhao);
        }

        public async Task DeleteTalhaoAsync(Talhao talhao)
        {
            await Initialize();
            var table = _client.GetSyncTable<Talhao>();
            await table.DeleteAsync(talhao);
        }

        public async Task<Talhao> FindTalhaoAsync(string talhaoid)
        {
            await Initialize();
            var table = _client.GetSyncTable<Talhao>();
            return await table.LookupAsync(talhaoid);
        }

        public async Task<IList<Talhao>> ListTalhaoAsync(string fazendaId)
        {
            await Initialize();
            var table = _client.GetSyncTable<Talhao>();
            var query = table.CreateQuery().Where(t => t.FazendaId == fazendaId).OrderBy(t => t.Nome);
            return await query.ToListAsync();
        }

        public async Task<bool> TalhaoHasAnalisesAsync(string talhaoid)
        {
            await Initialize();
            var table = _client.GetSyncTable<Analise>();
            var query = table.Where(a => a.TalhaoId == talhaoid);
            var list = await query.ToListAsync();
            return list.Any();
        }

        #endregion

        #region Metodos para análise

        public async Task InsertAnaliseAsync(Analise analise)
        {
            await Initialize();
            var table = _client.GetSyncTable<Analise>();
            await table.InsertAsync(analise);
        }

        public async Task UpdateAnaliseAsync(Analise analise)
        {
            await Initialize();
            var table = _client.GetSyncTable<Analise>();
            await table.UpdateAsync(analise);
        }

        public async Task DeleteAnaliseAsync(Analise analise)
        {
            await Initialize();
            var table = _client.GetSyncTable<Analise>();
            await table.DeleteAsync(analise);
        }

        public async Task<Analise> FindAnaliseAsync(string analiseid)
        {
            await Initialize();
            var table = _client.GetSyncTable<Analise>();
            return await table.LookupAsync(analiseid);
        }

        public async Task<IList<Analise>> ListAnaliseAsync()
        {
            await Initialize();
            var table = _client.GetSyncTable<Analise>();
            var query = table.CreateQuery();//.PerUser();
            return await query.OrderBy(a => a.Identificacao).ToListAsync();
        }

        #endregion
    }
}