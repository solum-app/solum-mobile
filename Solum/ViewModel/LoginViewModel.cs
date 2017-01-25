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

        public ICommand ShowRegisterPageCommand => _showRegisterPageCommand ?? (_showRegisterPageCommand = new Command(ShowRegisterPage));
        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new Command(DoLogin));


        #endregion

        #region Funções

        public async void DoLogin()
        {
            var binding = new LoginBinding
            {
                Username = Username?.Trim(),
                Password = Password?.Trim()
            };

            if (!binding.IsValid)
            {
                NullEntriesMessage.ToDisplayAlert(NullEntriesTitle, MessageType.Error);
                return;
            }

            InLogin = true;
            var authService = AuthService.Instance;
            var login = await authService.Login(binding);
            InLogin = false;
            if (login == AuthResult.LoginSuccessFully)
            {
                LoginSuccessMessage.ToToast(ToastNotificationType.Success);
                Application.Current.MainPage = new RootPage();
                Dispose();
            }
            else
            {
                InvalidCredentialsMessage.ToDisplayAlert(InvalidCredentialsTitle, MessageType.Error);
            }
        }

        public async void ShowRegisterPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new CadastroPage(), true);
                IsBusy = false;
            }
        }

        #endregion
    }
}