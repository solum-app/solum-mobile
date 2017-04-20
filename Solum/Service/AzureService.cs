using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Solum.Helpers;
using Solum.Models;

namespace Solum.Service
{
    public class AzureService<T> where T : EntityData
    {
        protected MobileServiceClient Client;
        protected IMobileServiceSyncTable<T> Table;
        protected AzureService()
        {
            Client = new MobileServiceClient(Settings.BaseUri);
        }

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var store = new MobileServiceSQLiteStore(Settings.DBPath);
            store.DefineTable<Estado>();
            store.DefineTable<Cidade>();
            if (Client == null)
                Client = new MobileServiceClient(Settings.BaseUri);
            Table = Client.GetSyncTable<T>();
            await Client.SyncContext.InitializeAsync(store);
        }
    }
}