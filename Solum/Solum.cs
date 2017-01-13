using System.Linq;
using Realms;
using Solum.Effects;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Solum
{
    public class App : Application
    {
        public App()
        {
            
            var isUsuarioLogado = VerificaLogin();
            if (!isUsuarioLogado)
            {
                var color = Color.Black;
                if (Device.OS == TargetPlatform.Android)
                    DependencyService.Get<IStatusBarColor>().SetColor(color);
                MainPage = new NavigationPage(new LoginPage())
                {
                    BackgroundColor = Color.Transparent,
                    BarTextColor = Color.Black
                };
            }
            else
            {
                MainPage = new RootPage();
            }
        }

        private bool VerificaLogin()
        {
            var loggedUser = Realm.GetInstance().All<Usuario>().FirstOrDefault();
            return loggedUser != default(Usuario);
        }
    }
}