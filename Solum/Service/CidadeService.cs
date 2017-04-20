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
    public class CidadeService : AzureService<Cidade>
    {
        private static CidadeService _instance;
        public static CidadeService Instance => _instance ?? (_instance = new CidadeService());

        private CidadeService() { }

        //public async Task SyncCidades()
        //{
        //    await Initialize();
        //    table = Client.GetSyncTable<Cidade>();
        //    try
        //    {
        //        if (!CrossConnectivity.Current.IsConnected)
        //            return;
        //        InSync = true;
        //        var q = table.CreateQuery();
        //        await table.PullAsync($"CidadesSync", q);

        //        //await Client.SyncContext.PushAsync();
        //        Settings.CidadesLoaded = true;
        //        InSync = false;
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

        public async Task<IList<Cidade>> GetCidadesFrom(string estadoId)
        {
            await Initialize();
            var cidades = await Table.Where(c => c.EstadoId.Equals(estadoId)).OrderBy(c => c.Nome).ToListAsync();
            return cidades;
        }

        public async Task<Cidade> FindAsync(string cidadeId)
        {
            await Initialize();
            return await Table.LookupAsync(cidadeId);

        }
    }
}