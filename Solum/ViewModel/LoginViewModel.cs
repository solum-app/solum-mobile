using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Solum.Auth;
using Solum.Helpers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(INavigation navigation) : base(navigation)
        {
            _userDialogs = DependencyService.Get<IUserDialogs>();
        }

        #region Private Propeties

        private ICommand _showRegisterPageCommand;
        private ICommand _loginCommand;
        private ICommand _showForgtPasswordPageCommand;
        private ICommand _facebookLoginCommand;
        private string _password;
        private string _username;
        private bool _inLogin;
        private IUserDialogs _userDialogs;

        #endregion

        #region Binding Properties

        public string Username
        {
            get { return _username; }
            set { SetPropertyChanged(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetPropertyChanged(ref _password, value); }
        }

        public bool InLogin
        {
            get { return _inLogin; }
            set { SetPropertyChanged(ref _inLogin, value); }
        }

        #endregion

        #region Commands

        public ICommand ShowForgotPasswordPageCommand
            => _showForgtPasswordPageCommand ?? (_showForgtPasswordPageCommand = new Command(ShowForgotPasswordPage));

        public ICommand ShowRegisterPageCommand
			=> _showRegisterPageCommand ?? (_showRegisterPageCommand = new Command(async ()=> await ShowRegisterPage()));

		public ICommand LoginCommand
			=> _loginCommand ?? (_loginCommand = new Command(async ()=> await DoLogin()));

        #endregion

        #region Functions

		private async Task DoLogin() {
			if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password)) {
                await _userDialogs.DisplayAlert(MessagesResource.LoginCredenciaisNulas);
                return;
            }

            IsBusy = true;

            var result = await AuthService.Instance.LoginAsync(Username, Password);
     
            if (result.IsSuccess)
			{
                AzureService.Instance.SetCredentials(result.Data);
                await AzureService.Instance.SynchronizeAllAsync();
                IsBusy = false;
                Application.Current.MainPage = new RootPage();
            } else {
                IsBusy = false;
                await _userDialogs.DisplayAlert(result.Message);
            }
        }

        public async Task GoogleLogin(GoogleCredentials credentials)
        {
            var result = await AuthService.Instance.GoogleLoginAsync(credentials);
            if (result.IsSuccess) {
                AzureService.Instance.SetCredentials(result.Data);
                await AzureService.Instance.SynchronizeAllAsync();
                IsBusy = false;
                Application.Current.MainPage = new RootPage();
            } else {
                IsBusy = false;
                await _userDialogs.DisplayAlert(result.Message);
            }
        }

        private void ShowForgotPasswordPage()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                //Device.OpenUri(new Uri("https://solum"));
                IsBusy = false;
            }
        }

		private async Task ShowRegisterPage()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new CadastroPage());
                IsBusy = false;
            }
        }

        #endregion
    }
}