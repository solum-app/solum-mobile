using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Solum.Auth;
using Solum.Handlers;
using Solum.Helpers;
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
        }

        #region Private Propeties

        private ICommand _showRegisterPageCommand;
        private ICommand _loginCommand;
        private ICommand _showForgtPasswordPageCommand;
        private string _password;
        private string _username;
        private bool _inLogin;

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
            => _showRegisterPageCommand ?? (_showRegisterPageCommand = new Command(ShowRegisterPage));

        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new Command(DoLogin));

        #endregion

        #region Functions

        private async void DoLogin()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessagesResource.LoginCredenciaisNulas.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            var binding = new LoginBinding
            {
                Email = Username.Trim(),
                Password = Password.Trim()
            };

            try
            {
                var provider = DependencyService.Get<IAuthentication>();
                if (App.Service.Client == null)
                    await App.Service.Initialize();
                var obj = JObject.FromObject(binding);
                var user = await provider.LoginAsync(App.Service.Client, "custom", obj);
                Settings.Token = user.MobileServiceAuthenticationToken;
                Settings.UserId = user.UserId;
                App.Current.MainPage = new RootPage();
                IsBusy = false;
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                Debug.WriteLine($"[ExecuteLoginCommand] Error = {ex.Message}");
            }
            catch (Exception  ex)
            {
                Debug.WriteLine($"[ExecuteLoginCommand] Error = {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ShowForgotPasswordPage()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                Device.OpenUri(new Uri("http://192.168.0.13/solum"));
                IsBusy = false;
            }
        }

        private async void ShowRegisterPage()
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