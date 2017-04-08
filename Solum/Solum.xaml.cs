
using System;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using Solum.Abstractions;
using Solum.Helpers;
using Solum.Pages;
using Solum.Services;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Solum
{
    public partial class App : Application
    {
        public static MobileServiceClient Client { get; } = new MobileServiceClient(new Uri(Locations.AppServiceUrl));
        public App()
        {
            InitializeComponent();
            ServiceLocator.Instance.Add<ICloudService, AzureCloudService>();
            var logged = AccountStore.Create().FindAccountsForService("identity").FirstOrDefault();
            if (logged == null)
                MainPage = new NavigationPage(new LoginPage())
                {
                    BackgroundColor = Color.Transparent,
                    BarTextColor = Color.Black
                };
            else
                MainPage = new RootPage();
        }
    }
}