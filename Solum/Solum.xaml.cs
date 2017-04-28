using Microsoft.WindowsAzure.MobileServices;
using Solum.Auth;
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
		public static MobileServiceClient Client { get; } = new MobileServiceClient(Settings.BaseUri);

		public App()
		{
			InitializeComponent();
		    AzureService.Instance.Sync();
			var authr = DependencyService.Get<IAuthentication>();
			var isLogged = authr.IsLogged();
			if (isLogged)
				MainPage = new RootPage();
			else
				MainPage = new NavigationPage(new LoginPage())
				{
					BackgroundColor = Color.Transparent,
					BarTextColor = Color.Black
				};
		}
	}
}