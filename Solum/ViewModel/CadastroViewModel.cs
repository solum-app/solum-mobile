using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Realms;
using Solum.Models;
using Solum.Remotes.Results;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class CadastroViewModel : BaseViewModel
    {
        private ICommand _registerCommand;
        private ICommand _updateCidadesCommand;
        private ICommand _voltarCommand;
        private IList<Cidade> _cidades = new List<Cidade>();
        private IList<Estado> _estados = new List<Estado>();
        private Cidade _cidadeSelected;
        private Estado _estadoSelected;
        private string _name;
        private string _password;
        private string _confirmPassword;
        private string _email;
        private bool _isCarregandoEstados = true;
        private bool _isCidadesCarregadas;

        private AuthService _authService;

        public CadastroViewModel(INavigation navigation) : base(navigation)
        {
            _authService = new AuthService();
            CarregarEstados();
        }

        public string Name
        {
            get { return _name; }
            set { SetPropertyChanged(ref _name, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetPropertyChanged(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetPropertyChanged(ref _password, value); }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetPropertyChanged(ref _confirmPassword, value); }
        }

        public Estado EstadoSelected
        {
            get { return _estadoSelected; }
            set { SetPropertyChanged(ref _estadoSelected, value); }
        }

        public Cidade CidadeSelected
        {
            get { return _cidadeSelected; }
            set { SetPropertyChanged(ref _cidadeSelected, value); }
        }

        public IList<Estado> Estados
        {
            get { return _estados; }
            set { SetPropertyChanged(ref _estados, value); }
        }

        public IList<Cidade> Cidades
        {
            get { return _cidades; }
            set { SetPropertyChanged(ref _cidades, value); }
        }

        public bool IsCarregandoEstados
        {
            get { return _isCarregandoEstados; }
            set { SetPropertyChanged(ref _isCarregandoEstados, value); }
        }

        public bool IsCidadesCarregadas
        {
            get { return _isCidadesCarregadas; }
            set { SetPropertyChanged(ref _isCidadesCarregadas, value); }
        }

        public ICommand RegisterCommand => _registerCommand ?? (_registerCommand = new Command(Cadastrar));

        public ICommand UpdateCidadesCommand => _updateCidadesCommand ?? (_updateCidadesCommand = new Command(AtualizarCidades));

        public ICommand VoltarCommand => _voltarCommand ?? (_voltarCommand = new Command(Voltar));

        public async void Cadastrar()
        {
            var registerBinding = new RegisterBinding
            {
                Nome = Name,
                Email = Email,
                Password = Password,
                ConfirmPassword = ConfirmPassword
            };

            if (CidadeSelected == default(Cidade))
            {
                MessagingCenter.Send(this, "CidadeNull", "Selecione uma cidade");
                return;
            }

            registerBinding.CidadeId = CidadeSelected.Id;

            if (!registerBinding.IsValid)
            {
                MessagingCenter.Send(this, "NullEntrys", "Preencha todos os campos, selecione o Estado e a Cidade que reside.");
                return;
            }
            
            var result = await _authService.Register(registerBinding);

            if (result == RegisterResult.RegisterSuccefully)
            {
                MessagingCenter.Send(this, "RegisterSuccessful", "Seu cadastro foi realizado com sucesso. Em instantes receberá um email para confirmar sua conta");
                await Navigation.PopAsync(true);
            }

            if (result == RegisterResult.RegisterUnsuccessfully)
            {
                MessagingCenter.Send(this, "RegisterUnsuccessful", "Seu cadastro não foi realizado. E-Mail já cadastrado!");
            }
        }

        public void CarregarEstados()
        {
            var realm = Realm.GetInstance();
            Estados = realm.All<Estado>().OrderBy(x => x.Nome).ToList();
            IsCarregandoEstados = false;
        }

        public void AtualizarCidades()
        {
            var realm = Realm.GetInstance();
            Cidades = realm.All<Cidade>().Where(x => x.EstadoId.Equals(EstadoSelected.Id)).OrderBy(x => x.Nome).ToList();
            IsCidadesCarregadas = true;
        }

        public async void Voltar()
        {
            await Navigation.PopAsync();
        }
    }
}