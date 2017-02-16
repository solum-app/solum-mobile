using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using EmailValidation;
using Realms;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Messages;
using Solum.Models;
using Solum.Remotes.Results;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class CadastroViewModel : BaseViewModel
    {
        public CadastroViewModel(INavigation navigation) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _authService = AuthService.Instance;
            IsEstadosLoaded = false;
            LoadEstados();
        }

        #region Private Properties

        private ICommand _updateCidadesCommand;
        private ICommand _registerCommand;
        private ICommand _backCommand;

        private IList<Estado> _estados;
        private IList<Cidade> _cidades;
        private Estado _estadoSelected;
        private Cidade _cidadeSelected;

        private string _name;
        private string _email;
        private string _password;
        private string _confirmPassword;

        private bool _inRegistering;
        private bool _isCidadesLoaded;
        private bool _isEstadosLoaded;

        private readonly AuthService _authService;
        private readonly Realm _realm;

        #endregion

        #region Binding Properties
        
        public string Name
        {
            get { return _name; }
            set { SetPropertyChanged(ref _name, value.Trim()); }
        }

        public string Email
        {
            get { return _email; }
            set { SetPropertyChanged(ref _email, value.Trim()); }
        }

        public string Password
        {
            get { return _password; }
            set { SetPropertyChanged(ref _password, value.Trim()); }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetPropertyChanged(ref _confirmPassword, value.Trim()); }
        }

        public IList<Estado> Estados
        {
            get { return _estados; }
            set { SetPropertyChanged(ref _estados, value); }
        }

        public Estado EstadoSelected
        {
            get { return _estadoSelected; }
            set { SetPropertyChanged(ref _estadoSelected, value); }
        }

        public bool IsEstadosLoaded
        {
            get { return _isEstadosLoaded; }
            set { SetPropertyChanged(ref _isEstadosLoaded, value); }
        }

        public IList<Cidade> Cidades
        {
            get { return _cidades; }
            set { SetPropertyChanged(ref _cidades, value); }
        }

        public Cidade CidadeSelected
        {
            get { return _cidadeSelected; }
            set { SetPropertyChanged(ref _cidadeSelected, value); }
        }

        public bool IsCidadesLoaded
        {
            get { return _isCidadesLoaded; }
            set { SetPropertyChanged(ref _isCidadesLoaded, value); }
        }

        public bool InRegistering
        {
            get { return _inRegistering; }
            set { SetPropertyChanged(ref _inRegistering, value); }
        }

        #endregion

        #region Commands

        public ICommand RegisterCommand => _registerCommand ?? (_registerCommand = new Command(Register));

        public ICommand UpdateCidadesCommand
            => _updateCidadesCommand ?? (_updateCidadesCommand = new Command(UpdateCidades));

        public ICommand BackCommand => _backCommand ?? (_backCommand = new Command(Back));

        #endregion

        #region Functions

        public void LoadEstados()
        {
            Estados = _realm.All<Estado>().OrderBy(x => x.Nome).ToList();
            IsEstadosLoaded = true;
        }

        public void UpdateCidades()
        {
            Cidades =
                _realm.All<Cidade>().Where(x => x.EstadoId.Equals(EstadoSelected.Id)).OrderBy(x => x.Nome).ToList();
            IsCidadesLoaded = true;
        }

        public async void Register()
        {
            InRegistering = true;

            if (string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(ConfirmPassword))
            {
                RegisterMessages.NullEntriesMessage.ToDisplayAlert(MessageType.Erro);
                InRegistering = false;
                return;
            }

            if (!EmailValidator.Validate(Email))
            {
                RegisterMessages.InvalidEmail.ToDisplayAlert(MessageType.Erro);
                InRegistering = false;
                return;
            }

            if (Password.Length < 6)
            {
                RegisterMessages.PasswordToShort.ToDisplayAlert(MessageType.Erro);
                InRegistering = false;
                return;
            }

            if (!Password.Equals(ConfirmPassword))
            {
                RegisterMessages.PasswordIsntMatch.ToDisplayAlert(MessageType.Erro);
                InRegistering = false;
                return;
            }

            if (CidadeSelected == null)
            {
                RegisterMessages.CidadeIsntSelected.ToDisplayAlert(MessageType.Erro);
                InRegistering = false;
                return;
            }

            var registerBinding = new RegisterBinding
            {
                Nome = Name,
                Email = Email,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                CidadeId = CidadeSelected.Id
            };

            var result = await _authService.Register(registerBinding);

            if (result == RegisterResult.RegisterSuccefully)
            {
                RegisterMessages.Success.ToToast(ToastNotificationType.Sucesso);
                InRegistering = false;
                await Navigation.PopAsync();
            }
            else
            {
                RegisterMessages.Unsucces.ToDisplayAlert(MessageType.Falha);
                InRegistering = false;
            }
        }


        public async void Back()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PopAsync();
                IsBusy = false;
            }
        }

        #endregion
    }
}