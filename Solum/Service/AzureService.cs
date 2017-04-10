using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Solum.Auth;
using Solum.Helpers;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

[assembly: Dependency(typeof(AzureService))]

namespace Solum.Service
{
    public class AzureService
    {
        private IMobileServiceSyncTable<Estado> _estadoTable;

        public MobileServiceClient Client { get; set; }
        public static bool UseAuth { get; set; } = false;

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = Settings.BaseUri;
            Client = new MobileServiceClient(appUrl);

            var path = "Solum.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Estado>();
            await Client.SyncContext.InitializeAsync(store);
            _estadoTable = Client.GetSyncTable<Estado>();
        }

        public async Task SyncEstados()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await _estadoTable.PullAsync("TodosOsEstados", _estadoTable.CreateQuery());
                await Client.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Não foi possível sincronizar estados");
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<IEnumerable<Estado>> GetEstados()
        {
            await Initialize();
            await SyncEstados();
            return await _estadoTable.OrderBy(c => c.Nome).ToEnumerableAsync();
        }

        //public async Task<Estado> AddCoffee(bool atHome)
        //{
        //    await Initialize();

        //    var coffee = new Estado
        //    {
        //        DateUtc = DateTime.UtcNow,
        //        MadeAtHome = atHome,
        //        OS = Device.OS.ToString()
        //    };

        //    await _estadoTable.InsertAsync(coffee);

        //    await SyncEstados();
        //    //return coffee
        //    return coffee;
        //}


        public async Task<bool> LoginAsync()
        {
            await Initialize();

            var auth = DependencyService.Get<IAuthentication>();
            var user = await auth.LoginAsync(Client, MobileServiceAuthenticationProvider.Facebook);
            if (user == null)
            {
                Settings.Token = string.Empty;
                Settings.UserId = string.Empty;
                Device.BeginInvokeOnMainThread(
                    async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("Login Error",
                            "Unable to login, please try again", "OK");
                    });
                return false;
            }
            Settings.Token = user.MobileServiceAuthenticationToken;
            Settings.UserId = user.UserId;

            return true;
        }
    }
}