using System;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Solum.Auth;
using Solum.Handlers;
using Solum.Helpers;
using Solum.Models;
using Solum.Pages;
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
        private ICommand _facebookLoginCommand;
        //private ICommand _googleLoginCommand;
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

        public ICommand FacebookLoginCommand
            => _facebookLoginCommand ?? (_facebookLoginCommand = new Command(FacebookLogin));

        //public ICommand GoogleLoginCommand => _googleLoginCommand ?? (_googleLoginCommand = new Command(GoogleLogin));

        #endregion

        #region Functions

        private async void DoLogin()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
				IsBusy = false;
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
                var obj = JObject.FromObject(binding);
                var user = await provider.LoginAsync(new MobileServiceClient(Settings.BaseUri), Settings.AuthProvider, obj);
                if(user != null)
                    Application.Current.MainPage = new RootPage();
                else 
                    "Não foi possível realizar login. Tente novamente mais tarde".ToDisplayAlert(MessageType.Aviso);
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

        private async void FacebookLogin()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                var provider = DependencyService.Get<IAuthentication>();
                var user = await provider.LoginAsync(new MobileServiceClient(Settings.BaseUri), MobileServiceAuthenticationProvider.Facebook);
                if (user != null)
                    Application.Current.MainPage = new RootPage();
                IsBusy = false;
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                Debug.WriteLine($"[ExecuteLoginCommand] Error = {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ExecuteLoginCommand] Error = {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        //private async void GoogleLogin()
        //{
        //    if (IsBusy)
        //        return;
        //    IsBusy = true;

        //    try
        //    {
        //        var provider = DependencyService.Get<IAuthentication>();
        //        var user = await provider.LoginAsync(new MobileServiceClient(Settings.BaseUri), MobileServiceAuthenticationProvider.Google);
        //        Application.Current.MainPage = new RootPage();
        //        IsBusy = false;
        //    }
        //    catch (MobileServiceInvalidOperationException ex)
        //    {
        //        Debug.WriteLine($"[ExecuteLoginCommand] Error = {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"[ExecuteLoginCommand] Error = {ex.Message}");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        private void ShowForgotPasswordPage()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                //Device.OpenUri(new Uri("https://solum"));
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