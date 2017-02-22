using System.Windows.Input;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Solum.Remotes.Results;
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

            InLogin = true;

            var authService = AuthService.Instance;
            var login = await authService.Login(binding);

            InLogin = false;

            if (login == AuthResult.LoginSuccessFully)
            {
                MessagesResource.LoginSucesso.ToToast();
                Application.Current.MainPage = new RootPage();
                Dispose();
            }

            else if (login == AuthResult.ServerUnrecheable)
            {
                MessagesResource.ServidorIndisponivel.ToDisplayAlert(MessageType.Aviso);
                return;
            }
            else
            {
                MessagesResource.LoginCredenciaisErradas.ToDisplayAlert(MessageType.Erro);
                return;
            }
        }

        private void ShowForgotPasswordPage()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                MessagesResource.NaoImplementado.ToToast(ToastNotificationType.Info);
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