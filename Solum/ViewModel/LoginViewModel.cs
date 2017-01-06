using System.Windows.Input;
using Solum.Models;
using Solum.Pages;
using Solum.Remotes;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private ICommand _cadastrarCommand;
        private bool _inLoggin; // variável para controle do activity indicator na tela de login;
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

        public bool InLoggin
        {
            get { return _inLoggin; }
            set { SetPropertyChanged(ref _inLoggin, value); }
        }

        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new Command(Logar));
        public ICommand CadastrarCommand => _cadastrarCommand ?? (_cadastrarCommand = new Command(Cadastrar));


        public async void Logar()
        {
            var accontRemote = new AccountRemote();
            var loginBinding = new LoginBinding {Username = _username, Password = _password};
            _inLoggin = true;
            var result = await accontRemote.Login(loginBinding);
            _inLoggin = false;
            //if (result)
            //    await Navigation.PushAsync(new RootPage(), true);
            // mostrar alerta na tela 
        }

        public async void Cadastrar()
        {
            await Navigation.PushAsync(new CadastroPage(), true);
        }
    }
}