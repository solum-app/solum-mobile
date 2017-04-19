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
    public class EstadoService : AzureService
    {
        private static EstadoService _instance;
        private IMobileServiceSyncTable<Estado> _estadoTable;

        private EstadoService()
        {
        }

        public static EstadoService Instance => _instance ?? (_instance = new EstadoService());
        public static bool InSync { get; private set; }

        public async Task SyncEstados()
        {
            await Initialize();
            _estadoTable = App.Client.GetSyncTable<Estado>();
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;
                InSync = true;
                await _estadoTable.PullAsync("TodosOsEstados", _estadoTable.CreateQuery());
                //await App.Client.SyncContext.PushAsync();
                InSync = false;
                Settings.EstadosLoaded = true;
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

        public async Task<ICollection<Estado>> GetEstados()
        {
            await Initialize();
            return await _estadoTable.OrderBy(c => c.Nome).ToListAsync();
        }
    }
}