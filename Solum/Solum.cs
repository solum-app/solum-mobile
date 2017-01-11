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
                MainPage = new NavigationPage(new LoginPage());
            else
                MainPage = new RootPage();
        }

        private bool VerificaLogin()
        {
            var dataService = new UserDataService();
            var loggedUser = dataService.GetLoggedUser();
            return loggedUser != default(Usuario);
        }
    }
}