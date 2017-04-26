using System.Collections.Generic;
using System.Net.Http;
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
            LoadEstados();
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
            get => _name;
            set => SetPropertyChanged(ref _name, value.Trim());
        }

        public string Email
        {
            get => _email;
            set => SetPropertyChanged(ref _email, value.Trim());
        }

        public string Password
        {
            get => _password;
            set => SetPropertyChanged(ref _password, value.Trim());
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetPropertyChanged(ref _confirmPassword, value.Trim());
        }

        public ICollection<Estado> Estados
        {
            get => _estados;
            set => SetPropertyChanged(ref _estados, value);
        }

        public Estado EstadoSelected
        {
            get => _estadoSelected;
            set => SetPropertyChanged(ref _estadoSelected, value);
        }

        public bool IsEstadosLoaded
        {
            get => _isEstadosLoaded;
            set => SetPropertyChanged(ref _isEstadosLoaded, value);
        }

        public ICollection<Cidade> Cidades
        {
            get => _cidades;
            set => SetPropertyChanged(ref _cidades, value);
        }

        public Cidade CidadeSelected
        {
            get => _cidadeSelected;
            set => SetPropertyChanged(ref _cidadeSelected, value);
        }

        public bool IsCidadesLoaded
        {
            get => _isCidadesLoaded;
            set => SetPropertyChanged(ref _isCidadesLoaded, value);
        }

        public bool InRegistering
        {
            get => _inRegistering;
            set => SetPropertyChanged(ref _inRegistering, value);
        }



        public ICommand RegisterCommand => _registerCommand ?? (_registerCommand = new Command(Register));

        public ICommand UpdateCidadesCommand
            => _updateCidadesCommand ?? (_updateCidadesCommand = new Command(UpdateCidades));

        public ICommand BackCommand => _backCommand ?? (_backCommand = new Command(Back));



        public async void LoadEstados()
        {
            Estados = await AzureService.Instance.ListEstadosAsync();
            IsEstadosLoaded = true;
        }

        public async void UpdateCidades()
        {
            Cidades =  await AzureService.Instance.ListCidadesAsync(EstadoSelected.Id);
            IsCidadesLoaded = true;
        }

        public async void Register()
        {
            if (IsNotBusy)
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(ConfirmPassword))
                {
                    MessagesResource.CamposVazios.ToDisplayAlert(MessageType.Info);
                    return;
                }

                if (!EmailValidator.Validate(Email))
                {
                    MessagesResource.UsuarioCadastroEmailInvalido.ToDisplayAlert(MessageType.Info);
                    return;
                }

                if (Password.Length < 6)
                {
                    MessagesResource.UsuarioCadastroSenhaCurta.ToDisplayAlert(MessageType.Info);
                    return;
                }

                if (!Password.Equals(ConfirmPassword))
                {
                    MessagesResource.UsuarioCadastroSenhasNaoConferem.ToDisplayAlert(MessageType.Info);
                    return;
                }

                if (CidadeSelected == null)
                {
                    MessagesResource.UsuarioCadastroCidadeNula.ToDisplayAlert(MessageType.Info);
                    return;
                }

                InRegistering = true;

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
                    $"Seu cadastro não foi realizado com sucesso. Motivo: {result.ReasonPhrase}".ToDisplayAlert();
                    IsBusy = false;
                }
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

    }
}