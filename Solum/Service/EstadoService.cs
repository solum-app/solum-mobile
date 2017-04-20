using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Solum.Models;

namespace Solum.Service
{
    public class EstadoService : AzureService<Estado>
    {
        private static EstadoService _instance;

        private EstadoService()
        {
        }

        public static EstadoService Instance => _instance ?? (_instance = new EstadoService());
        //public static bool InSync { get; private set; }

        //public async Task SyncEstados()
        //{
        //    await Initialize();
        //    _estadoTable = Client.GetSyncTable<Estado>();
        //    try
        //    {
        //        if (!CrossConnectivity.Current.IsConnected)
        //            return;
        //        InSync = true;
        //        await _estadoTable.PullAsync("TodosOsEstados", _estadoTable.CreateQuery());
        //        //await App.Client.SyncContext.PushAsync();
        //        InSync = false;
        //        Settings.EstadosLoaded = true;
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Não foi possível sincronizar estados");
        //        Debug.WriteLine(ex.Message);
        //    }
        //}

        public async Task<IList<Estado>> GetEstados()
        {
            await Initialize();
            var r = await Table.OrderBy(e => e.Nome).ToListAsync();
            return r;
        }
    }
}