using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using EmailValidation;
using Solum.Handlers;
using Solum.Helpers;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class CadastroViewModel : BaseViewModel
    {
        public CadastroViewModel(INavigation navigation) : base(navigation)
        {
			Task.Run(LoadEstados);
        }

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
        private bool _inRegistering;
        private bool _isCidadesLoaded;
        private bool _isEstadosLoaded;

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
            if (IsNotBusy)
            {
				IsBusy = true;

                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(ConfirmPassword))
                {
					IsBusy = false;
                    MessagesResource.CamposVazios.ToDisplayAlert(MessageType.Info);
                    return;
                }

                if (!EmailValidator.Validate(Email))
                {
					IsBusy = false;
                    MessagesResource.UsuarioCadastroEmailInvalido.ToDisplayAlert(MessageType.Info);
                    return;
                }

                if (Password.Length < 6)
                {
					IsBusy = false;
                    MessagesResource.UsuarioCadastroSenhaCurta.ToDisplayAlert(MessageType.Info);
                    return;
                }

                if (!Password.Equals(ConfirmPassword))
                {
					IsBusy = false;
                    MessagesResource.UsuarioCadastroSenhasNaoConferem.ToDisplayAlert(MessageType.Info);
                    return;
                }

                if (CidadeSelected == null)
                {
					IsBusy = false;
                    MessagesResource.UsuarioCadastroCidadeNula.ToDisplayAlert(MessageType.Info);
                    return;
                }

                var dict = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("nome", Name),
                    new KeyValuePair<string, string>("email", Email),
                    new KeyValuePair<string, string>("password", Password),
                    new KeyValuePair<string, string>("ConfirmPassword", ConfirmPassword),
                    new KeyValuePair<string, string>("CidadeId", CidadeSelected.Id)
                };

                var result = await new HttpClient().PostAsync($"{Settings.BaseUri}/.auth/register",
                    new FormUrlEncodedContent(dict));
                if (result.IsSuccessStatusCode)
                {
                    "Você foi cadastrado com sucesso".ToToast();
                    await Navigation.PopAsync();
					IsBusy = false;
                }
                else
                {
					IsBusy = false;
                    $"Seu cadastro não foi realizado com sucesso. Motivo: {result.ReasonPhrase}".ToDisplayAlert();
                }
            }
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