﻿using System.Threading.Tasks;
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
		public static string AppName { get { return "Solum"; } }

		public App()
		{
			InitializeComponent();

            var usuario = AuthService.Instance.GetCredentials;
            if (usuario == null)
            {
                AzureService.Instance.SetCredentials(usuario);
                MainPage = new NavigationPage(new BemVindoPage());
            } else {
                MainPage = new RootPage();
            }
		}
	}
}