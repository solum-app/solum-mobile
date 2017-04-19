using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
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
        protected AzureService() { }

        public async Task Initialize()
        {
            if (App.Client?.SyncContext?.IsInitialized ?? false)
                return;
			
			var store = new MobileServiceSQLiteStore(Settings.DBPath);
            store.DefineTable<Estado>();
            store.DefineTable<Cidade>();
            await App.Client.SyncContext.InitializeAsync(store);
        }

        public async Task<bool> LoginAsync()
        {
            await Initialize();
            var auth = DependencyService.Get<IAuthentication>();
            var user = await auth.LoginAsync(App.Client, MobileServiceAuthenticationProvider.Facebook);
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