using System.Linq;
using Realms;
using Solum.Models;
using Solum.Pages;
using Solum.Remotes;
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


            var r = Realm.GetInstance();
            if (!r.All<Estado>().Any())
                CarregarEstadoCidades(r);

            var isUsuarioLogado = VerificaLogin();
            if (!isUsuarioLogado)
                MainPage = new NavigationPage(new LoginPage())
                {
                    BackgroundColor = Color.Transparent,
                    BarTextColor = Color.Black
                };
            else
                MainPage = new RootPage();
        }

        private bool VerificaLogin()
        {
            var loggedUser = Realm.GetInstance().All<Usuario>().FirstOrDefault();
            return loggedUser != default(Usuario);
        }

        private void CarregarEstadoCidades(Realm realm)
        {
            var estados = new EstadoRemote().GetEstados().Result;
            using (var trans = realm.BeginWrite())
            {
                foreach (var e in estados)
                    realm.Add(e, true);
                trans.Commit();
            }
        }
    }
}