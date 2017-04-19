using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using Solum.Helpers;
using Solum.Models;

namespace Solum.Service
{
    public class CidadeService : AzureService
    {
        private static CidadeService _instance;
        public static CidadeService Instance => _instance ?? (_instance = new CidadeService());
        private CidadeService() { }

        private IMobileServiceSyncTable<Cidade> _cidadeTable;
        public static bool InSync { get; private set; }
        public async Task SyncCidades()
        {
            await Initialize();
            _cidadeTable = App.Client.GetSyncTable<Cidade>();
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;
                InSync = true;
                var q = _cidadeTable.CreateQuery();
                await _cidadeTable.PullAsync($"CidadesSync", q);

                //await App.Client.SyncContext.PushAsync();
                Settings.CidadesLoaded = true;
                InSync = false;
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Não foi possível sincronizar estados");
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<ICollection<Cidade>> GetCidadesFrom(string estadoId)
        {
            await SyncCidades();
            return await _cidadeTable.Where(c => c.EstadoId.Equals(estadoId)).OrderBy(c => c.Nome).ToListAsync();
        }
    }
}