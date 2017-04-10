using Solum.Helpers;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Solum
{
    public partial class App : Application
    {
        public static AzureService Service { get; set; } = new AzureService();
        public App()
        {
            InitializeComponent();
            Service.Initialize();
            if(string.IsNullOrEmpty(Settings.Token))
                MainPage = new LoginPage();
            else
                MainPage = new AnalisesPage();
        }
    }
}