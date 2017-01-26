using System.Windows.Input;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Solum.Remotes.Results;
using Solum.Service;
using Xamarin.Forms;
using static Solum.Messages.LoginMessages;

namespace Solum.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Propriedades Privadas

        private ICommand _showRegisterPageCommand;
        private ICommand _loginCommand;
        private ICommand _showForgtPasswordPageCommand;
        private string _password;
        private string _username;
        private bool _inLogin;

        #endregion


        public LoginViewModel(INavigation navigation) : base(navigation)
        {
        }

        #region Propriedades de Binding

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

        #region Commandos

        public ICommand ShowForgotPasswordPageCommand
            => _showForgtPasswordPageCommand ?? (_showForgtPasswordPageCommand = new Command(ShowForgotPasswordPage));
        public ICommand ShowRegisterPageCommand => _showRegisterPageCommand ?? (_showRegisterPageCommand = new Command(ShowRegisterPage));
        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new Command(DoLogin));


        #endregion

        #region Funções

        private async void DoLogin()
        {
            var binding = new LoginBinding
            {
                Username = Username?.Trim(),
                Password = Password?.Trim()
            };

            if (!binding.IsValid)
            {
                NullEntriesMessage.ToDisplayAlert(MessageType.Aviso);
                return;
            }

            InLogin = true;
            var authService = AuthService.Instance;
            var login = await authService.Login(binding);
            InLogin = false;
            if (login == AuthResult.LoginSuccessFully)
            {
                LoginSuccessMessage.ToToast(ToastNotificationType.Sucesso);
                Application.Current.MainPage = new RootPage();
                Dispose();
            }
            else if (login == AuthResult.ServerUnrecheable)
            {
                ServerUnrecheable.ToDisplayAlert(MessageType.Falha);
            }
            else 
            {
                InvalidCredentialsMessage.ToDisplayAlert(MessageType.Erro);
            }
        }

        private async void ShowForgotPasswordPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                "Não implementado ainda".ToToast();
                IsBusy = false;
            }
        }

        private async void ShowRegisterPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new CadastroPage());
                IsBusy = false;
            }
        }

        #endregion
    }
}