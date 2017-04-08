using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solum.Handlers;
using Solum.Models;
using Solum.Pages;
using Solum.Services;
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
                Username = Username.Trim(),
                Password = Password.Trim()
            };

            try
            {
                // "custom" é o que provider de login do mobile app service,
                // rota completa fica .auth/login/custom
                // mas não funciona e muito menos lança alguma exception.
                // pensei se o backend, mas no postman funciona e nem chora.
                // não sei o que fazer ...
                // as classes que tem novas ai eu pegue daqui -> https://adrianhall.github.io/develop-mobile-apps-with-csharp-and-azure/chapter2/custom/
                // que é o livro que o cara escreveu de como usar as parada tudo
                // mas no final eu acabei criando uma variável estática do MobileClient na class APP

                var user = await App.Client.LoginAsync("custom", JObject.FromObject(binding));
                //Application.Current.MainPage = new RootPage();
                //var lastOrDefault = Navigation.NavigationStack.LastOrDefault();
                //await Navigation.PushAsync(new RootPage());
                //Navigation.RemovePage(lastOrDefault);
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

            IsBusy = false;
            //InLogin = true;

            //var authService = AuthService.Instance;
            //var login = await authService.Login(binding);

            //InLogin = false;

            //if (login == AuthResult.LoginSuccessFully)
            //{
            //    MessagesResource.LoginSucesso.ToToast();
            //    Application.Current.MainPage = new RootPage();
            //    Dispose();
            //}

            //else if (login == AuthResult.ServerUnrecheable)
            //{
            //    MessagesResource.ServidorIndisponivel.ToDisplayAlert(MessageType.Aviso);
            //    return;
            //}
            //else
            //{
            //    MessagesResource.LoginCredenciaisErradas.ToDisplayAlert(MessageType.Erro);
            //    return;
            //}
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