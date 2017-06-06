using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Solum.Handlers;
using Solum.Helpers;
using Solum.Interfaces;
using Solum.Models;

namespace Solum.Service
{
    public class AzureService : IDataService
    {
        private static AzureService _instance;
        public static AzureService Instance = _instance ?? (_instance = new AzureService());

        MobileServiceUser _user;
        IMobileServiceSyncTable<Estado> _estadoTable;
        IMobileServiceSyncTable<Cidade> _cidadeTable;
        IMobileServiceSyncTable<Analise> _analiseTable;
        IMobileServiceSyncTable<Fazenda> _fazendaTable;
        IMobileServiceSyncTable<Talhao> _talhaoTable;

        public MobileServiceClient Client
        {
            get;
            set;
        }

		public async Task InitializeAsync()
		{
			if (Client != null)
				return;

			var store = new MobileServiceSQLiteStore(Settings.DBPath);
			store.DefineTable<Estado>();
			store.DefineTable<Cidade>();
			store.DefineTable<Analise>();
			store.DefineTable<Fazenda>();
			store.DefineTable<Talhao>();

            Client = new MobileServiceClient(Settings.BaseUri, new AuthHandler());
			await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            if (_user != null)
                Client.CurrentUser = _user;

			_estadoTable = Client.GetSyncTable<Estado>();
			_cidadeTable = Client.GetSyncTable<Cidade>();
			_analiseTable = Client.GetSyncTable<Analise>();
			_fazendaTable = Client.GetSyncTable<Fazenda>();
			_talhaoTable = Client.GetSyncTable<Talhao>();
		}


		public void SetCredentials(MobileServiceUser user)
		{
            _user = user;
            if (Client != null)
                Client.CurrentUser = user;
		}

		#region Sincronizacao

        public async Task SynchronizeAllAsync()
		{
            await Task.WhenAll(SynchronizeAnaliseAsync(), SynchronizeFazendaAsync(), SynchronizeTalhaoAsync());
		}

		public async Task ClearAllAsync()
		{
            await Task.WhenAll(_analiseTable.PurgeAsync(), _fazendaTable.PurgeAsync(), _talhaoTable.PurgeAsync());
		}

		public async Task SynchronizeAnaliseAsync()
		{
            await InitializeAsync();

			if (!CrossConnectivity.Current.IsConnected)
				return;

			try
			{
				string queryName = $"sync_{typeof(Analise).Name}";
                await _analiseTable.PullAsync(queryName, _analiseTable.CreateQuery());
                await _estadoTable.PullAsync("allEstado", _estadoTable.CreateQuery());
                await _cidadeTable.PullAsync("allCidades", _cidadeTable.CreateQuery());
			}
			catch (MobileServicePushFailedException ex)
			{
				if (ex.PushResult != null)
				{
					foreach (var result in ex.PushResult.Errors)
					{
                        await ResolveError<Analise>(result);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Got exception: {0}", ex.Message);
			}
		}

        public async Task SynchronizeFazendaAsync()
		{
            await InitializeAsync();

			if (!CrossConnectivity.Current.IsConnected)
				return;

			try
			{
				string queryName = $"sync_{typeof(Fazenda).Name}";
                await _fazendaTable.PullAsync(queryName, _fazendaTable.CreateQuery());
			}
			catch (MobileServicePushFailedException ex)
			{
				if (ex.PushResult != null)
				{
					foreach (var result in ex.PushResult.Errors)
					{
                        await ResolveError<Fazenda>(result);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Got exception: {0}", ex.Message);
			}
		}

		public async Task SynchronizeTalhaoAsync()
		{
            await InitializeAsync();

			if (!CrossConnectivity.Current.IsConnected)
				return;

			try
			{
				string queryName = $"sync_{typeof(Talhao).Name}";
                await _talhaoTable.PullAsync(queryName, _talhaoTable.CreateQuery());
			}
			catch (MobileServicePushFailedException ex)
			{
				if (ex.PushResult != null)
				{
					foreach (var result in ex.PushResult.Errors)
					{
                        await ResolveError<Talhao>(result);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Got exception: {0}", ex.Message);
			}
		}

        private async Task ResolveError<T>(MobileServiceTableOperationError result) where T : EntityData
		{
			if (result.Result == null || result.Item == null)
				return;

			var serverItem = result.Result.ToObject<T>();
			var localItem = result.Item.ToObject<T>();

			// Always take the client
            localItem.Version = serverItem.Version;
			await result.UpdateOperationAsync(JObject.FromObject(localItem));
		}

		#endregion

		#region Metodos para Estado

		public async Task<IList<Estado>> ListEstadosAsync()
		{
			await InitializeAsync();
			return await _estadoTable.OrderBy(e => e.Nome).ToListAsync();
		}

		public async Task<Estado> FindEstadoAsync(string estadoId)
		{
			await InitializeAsync();
            return await _estadoTable.LookupAsync(estadoId);
		}

		#endregion

		#region Metodos para Cidade

		public async Task<IList<Cidade>> ListCidadesAsync(string estadoId)
		{
			await InitializeAsync();
			return await _cidadeTable.Where(c => c.EstadoId == estadoId).OrderBy(c => c.Nome).ToListAsync();
		}

		public async Task<Cidade> FindCidadeAsync(string cidadeId)
		{
			await InitializeAsync();
			return await _cidadeTable.LookupAsync(cidadeId);
		}

		#endregion

		#region Metodos para fazenda

		public async Task AddOrUpdateFazendaAsync(Fazenda fazenda)
		{
			await InitializeAsync();
			if (string.IsNullOrEmpty(fazenda.Id))
			{
				fazenda.Id = Guid.NewGuid().ToString();
				await _fazendaTable.InsertAsync(fazenda);
			}
			await _fazendaTable.UpdateAsync(fazenda);
			SynchronizeFazendaAsync();
		}

		public async Task DeleteFazendaAsync(Fazenda fazenda)
		{
			await InitializeAsync();
			await _fazendaTable.DeleteAsync(fazenda);
            SynchronizeFazendaAsync();
		}

		public async Task<Fazenda> FindFazendaAsync(string fazendaid)
		{
			await InitializeAsync();
			return await _fazendaTable.LookupAsync(fazendaid);
		}

		public async Task<IList<Fazenda>> ListFazendaAsync()
		{
			await InitializeAsync();
            return await _fazendaTable.OrderBy(f => f.Nome).ToListAsync();
		}

		#endregion

		#region Metodos para Talhao

		public async Task AddOrUpdateTalhaoAsync(Talhao talhao)
		{
			await InitializeAsync();
			if (string.IsNullOrEmpty(talhao.Id))
			{
                talhao.Id = Guid.NewGuid().ToString();
				await _talhaoTable.InsertAsync(talhao);
			}
			await _talhaoTable.UpdateAsync(talhao);
            SynchronizeTalhaoAsync();
		}

        public async Task DeleteTalhaoAsync(Talhao talhao)
        {
            await InitializeAsync();
			await _talhaoTable.DeleteAsync(talhao);
            SynchronizeTalhaoAsync();
        }

        public async Task<Talhao> FindTalhaoAsync(string talhaoid)
        {
            await InitializeAsync();
			return await _talhaoTable.LookupAsync(talhaoid);
        }

        public async Task<IList<Talhao>> ListTalhaoAsync(string fazendaId)
        {
            await InitializeAsync();
			return await _talhaoTable.Where(t => t.FazendaId == fazendaId).OrderBy(t => t.Nome).ToListAsync();
        }

        public async Task<bool> TalhaoHasAnalisesAsync(string talhaoid)
        {
            await InitializeAsync();
			var list = await _analiseTable.Where(a => a.TalhaoId == talhaoid).ToListAsync();
            return list.Any();
        }

		#endregion

		#region Metodos para análise

		public async Task AddOrUpdateAnaliseAsync(Analise analise)
		{
			await InitializeAsync();
			if (string.IsNullOrEmpty(analise.Id))
			{
                analise.Id = Guid.NewGuid().ToString();
				await _analiseTable.InsertAsync(analise);
			}
			await _analiseTable.UpdateAsync(analise);
            SynchronizeAnaliseAsync();
		}

        public async Task DeleteAnaliseAsync(Analise analise)
        {
            await InitializeAsync();
			await _analiseTable.DeleteAsync(analise);
            SynchronizeAnaliseAsync();
        }

        public async Task<Analise> FindAnaliseAsync(string analiseid)
        {
            await InitializeAsync();
			return await _analiseTable.LookupAsync(analiseid);
        }

        public async Task<IList<Analise>> ListAnaliseAsync()
        {
            await InitializeAsync();
			return await _analiseTable.OrderBy(a => a.Identificacao).ToListAsync();
        }

        #endregion
    }
}