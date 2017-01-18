using System.Windows.Input;
using Solum.Models;
using Solum.Pages;
using Solum.Remotes.Results;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private ICommand _cadastrarCommand;
        private ICommand _loginCommand;
        private bool _inLogin;
        private string _password;
        private string _username;

        public LoginViewModel(INavigation navigation) : base(navigation)
        {
        }

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

        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new Command(Logar));
        public ICommand CadastrarCommand => _cadastrarCommand ?? (_cadastrarCommand = new Command(AbrirTelaCadastro));


        public async void Logar()
        {
            var loginBinding = new LoginBinding {Username = Username?.Trim(), Password = Password?.Trim()};
            if (!loginBinding.IsValid)
            {
                MessagingCenter.Send(this, "NullEntrys", "Preencha os campos Usuário e Senha!");
                return;
            }
            InLogin = true;
            var authService = AuthService.Instance;
            var login = await authService.Login(loginBinding);
            InLogin = false;
            if (login == AuthResult.LoginSuccessFully)
            {
                Application.Current.MainPage = new RootPage();
            }
            else
            {
                MessagingCenter.Send(this, "LoginError", "Verifique as Credenciais. Usuário ou Senha incorretos!");
            }
        }

        public async void AbrirTelaCadastro()
        {
            await Navigation.PushAsync(new CadastroPage(), true);
        }
    }
}