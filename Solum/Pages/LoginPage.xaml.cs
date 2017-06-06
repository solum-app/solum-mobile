using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Solum.Effects;
using Solum.Helpers;
using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class LoginPage : ContentPage
    {
        public Func<Task<GoogleCredentials>> GoogleSigInAsync
        {
            get;
            set;
        }

        private LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LoginViewModel(Navigation);
            NavigationPage.SetHasNavigationBar(this, false);
			if (Device.RuntimePlatform == "Android")
                DependencyService.Get<IStatusBarColor>()
                    .SetColor((Color) Application.Current.Resources["loginBackgroundDark"]);

			if (Device.RuntimePlatform == "iOS")
			{
				UsernameEntry.HeightRequest = PasswordEntry.HeightRequest = 40;
			}
        }

        private void UsernameEntryOnCompleted(object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }

        private void PasswordEntryOnCompleted(object sender, EventArgs eventArgs)
        {
            PasswordEntry.Unfocus();
            (BindingContext as LoginViewModel)?.LoginCommand.Execute(null);
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (!_viewModel.IsBusy)
            {
                _viewModel.IsBusy = true;
                var credentials = await GoogleSigInAsync();
                await _viewModel.GoogleLogin(credentials);
            }
        }
    }
}