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
        private string _aviso;
        private ICommand _cadastrarCommand;
        private bool _inLogin; // variável para controle do activity indicator na tela de login;
        private bool _isAvisoVisible;
        private ICommand _loginCommand;
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

        public string Aviso
        {
            get { return _aviso; }
            set { SetPropertyChanged(ref _aviso, value); }
        }

        public bool InLogin
        {
            get { return _inLogin; }
            set { SetPropertyChanged(ref _inLogin, value); }
        }


        public bool IsAvisoVisible
        {
            get { return _isAvisoVisible; }
            set { SetPropertyChanged(ref _isAvisoVisible, value); }
        }

        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new Command(Logar));
        public ICommand CadastrarCommand => _cadastrarCommand ?? (_cadastrarCommand = new Command(Cadastrar));


        public async void Logar()
        {
            IsAvisoVisible = false;
            var loginBinding = new LoginBinding {Username = Username, Password = Password};
            InLogin = true;
            var authService = new AuthService();
            var login = await authService.Login(loginBinding);
            InLogin = false;
            if (login == AuthResult.LoginSuccessFully)
                await Navigation.PushAsync(new RootPage(), true);
            Aviso =
                "Não foi possível realizar login, as credenciais podem estar inválidas, sua conta pode não estar confirmada ou não existe conexão com a internet";
            IsAvisoVisible = true;
        }

        public async void Cadastrar()
        {
            await Navigation.PushAsync(new CadastroPage(), true);
        }
    }
}