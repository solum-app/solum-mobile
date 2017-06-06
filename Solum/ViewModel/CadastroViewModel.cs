using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using EmailValidation;
using Solum.Handlers;
using Solum.Helpers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class CadastroViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private ICommand _updateCidadesCommand;
        private ICommand _registerCommand;
        private ICommand _backCommand;
        private ICollection<Estado> _estados;
        private ICollection<Cidade> _cidades;
        private Estado _estadoSelected;
        private Cidade _cidadeSelected;
        private string _name;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private bool _isCidadesLoaded;
        private bool _isEstadosLoaded;

		public CadastroViewModel(INavigation navigation) : base(navigation)
		{
			Task.Run(LoadEstados);
            _userDialogs = DependencyService.Get<IUserDialogs>();
		}

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

        public ICollection<Estado> Estados
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

        public ICollection<Cidade> Cidades
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

		public ICommand RegisterCommand 
			=> _registerCommand ?? (_registerCommand = new Command(async ()=> await Register()));

        public ICommand UpdateCidadesCommand
			=> _updateCidadesCommand ?? (_updateCidadesCommand = new Command(async ()=> await UpdateCidades()));

		public ICommand BackCommand 
			=> _backCommand ?? (_backCommand = new Command(async ()=> await Back()));

		public async Task LoadEstados()
        {
            Estados = await AzureService.Instance.ListEstadosAsync();
            IsEstadosLoaded = true;
        }

        public async Task UpdateCidades()
        {
            Cidades =  await AzureService.Instance.ListCidadesAsync(EstadoSelected.Id);
            IsCidadesLoaded = true;
        }

        public async Task Register()
        {

            if (string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(Email)
                || string.IsNullOrEmpty(Password)
                || string.IsNullOrEmpty(ConfirmPassword))
            {
                await _userDialogs.DisplayAlert(MessagesResource.CamposVazios);
                return;
            }

            if (!EmailValidator.Validate(Email))
            {
                await _userDialogs.DisplayAlert(MessagesResource.UsuarioCadastroEmailInvalido);
                return;
            }

            if (Password.Length < 6)
            {
                await _userDialogs.DisplayAlert(MessagesResource.UsuarioCadastroSenhaCurta);
                return;
            }

            if (!Password.Equals(ConfirmPassword))
            {
                await _userDialogs.DisplayAlert(MessagesResource.UsuarioCadastroSenhasNaoConferem);
                return;
            }

            //if (CidadeSelected == null)
            //{
            //    await _userDialogs.DisplayAlert(MessagesResource.UsuarioCadastroCidadeNula);
            //    return;
            //}

            IsBusy = true;

            var result = await AuthService.Instance.RegisterAsync(Name, Email, Password, CidadeSelected?.Id);

            if (result.IsSuccess)
            {
                _userDialogs.ShowToast(MessagesResource.CadastroSucesso);
                await Navigation.PopAsync();
            }
            else
            {
                await _userDialogs.DisplayAlert(result.Message);
            }

            IsBusy = false;
        }


		public async Task Back()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PopAsync();
                IsBusy = false;
            }
        }

    }
}