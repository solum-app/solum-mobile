using System.Linq;
using Realms;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Solum
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SyncService.CidadeEstadoSync();
            var isUsuarioLogado = Realm.GetInstance().All<Usuario>().Any();
            if (!isUsuarioLogado)
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