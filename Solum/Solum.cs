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
            var userdataservice = new UserDataService();
            var user = userdataservice.GetLoggedUser();
            if (user != default(Usuario))
                MainPage = new RootPage();
            else
                MainPage =new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}