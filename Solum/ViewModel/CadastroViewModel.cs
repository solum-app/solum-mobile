using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using EmailValidation;
using Realms;
using Solum.Models;
using Solum.Remotes.Results;
using Solum.Service;
using Xamarin.Forms;
using static Solum.Messages.RegisterMessages;

namespace Solum.ViewModel
{
    public class CadastroViewModel : BaseViewModel
    {
        private ICommand _atualizarCidadesCommand;
        private ICommand _registrarCommand;
        private ICommand _voltarCommand;

        private IList<Estado> _estados;
        private IList<Cidade> _cidades;
        private Estado _estadoSelecionado;
        private Cidade _cidadeSelecionada;

        private string _nome;
        private string _email;
        private string _senha;
        private string _confirmaSenha;

        private bool _inRegistering;
        private bool _isCidadesCarregadas;
        private bool _isEstadosCarregados;

        private readonly AuthService _authService;
        private readonly Realm _realm;

        public CadastroViewModel(INavigation navigation) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _authService = AuthService.Instance;
            IsEstadosCarregados = false;
            CarregarEstados();
        }

        public ICommand RegistrarCommand => _registrarCommand ?? (_registrarCommand = new Command(Cadastrar));

        public ICommand AtualizarCidadesCommand
            => _atualizarCidadesCommand ?? (_atualizarCidadesCommand = new Command(AtualizarCidades));

        public ICommand VoltarCommand => _voltarCommand ?? (_voltarCommand = new Command(Voltar));

        public string Nome
        {
            get { return _nome; }
            set { SetPropertyChanged(ref _nome, value.Trim()); }
        }

        public string Email
        {
            get { return _email; }
            set { SetPropertyChanged(ref _email, value.Trim()); }
        }

        public string Senha
        {
            get { return _senha; }
            set { SetPropertyChanged(ref _senha, value.Trim()); }
        }

        public string ConfirmaSenha
        {
            get { return _confirmaSenha; }
            set { SetPropertyChanged(ref _confirmaSenha, value.Trim()); }
        }

        public IList<Estado> Estados
        {
            get { return _estados; }
            set { SetPropertyChanged(ref _estados, value); }
        }

        public Estado EstadoSelecionado
        {
            get { return _estadoSelecionado; }
            set { SetPropertyChanged(ref _estadoSelecionado, value); }
        }

        public IList<Cidade> Cidades
        {
            get { return _cidades; }
            set { SetPropertyChanged(ref _cidades, value); }
        }

        public Cidade CidadeSelecionada
        {
            get { return _cidadeSelecionada; }
            set { SetPropertyChanged(ref _cidadeSelecionada, value); }
        }

        public bool IsEstadosCarregados
        {
            get { return _isEstadosCarregados; }
            set { SetPropertyChanged(ref _isEstadosCarregados, value); }
        }

        public bool IsCidadesCarregadas
        {
            get { return _isCidadesCarregadas; }
            set { SetPropertyChanged(ref _isCidadesCarregadas, value); }
        }

        public bool InRegistering
        {
            get { return _inRegistering; }
            set { SetPropertyChanged(ref _inRegistering, value); }
        }

        public async void Cadastrar()
        {
            InRegistering = true;

            if (string.IsNullOrEmpty(Nome) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Senha) || string.IsNullOrEmpty(ConfirmaSenha))
            {
                MessagingCenter.Send(this, EntryNullValuesTitle);
                InRegistering = false;
                return;
            }

            if (!EmailValidator.Validate(Email))
            {
                MessagingCenter.Send(this, InvalidEmailTitle);
                InRegistering = false;
                return;
            }

            if (!Senha.Equals(ConfirmaSenha))
            {
                MessagingCenter.Send(this, PasswordIsntMatchTitle);
                InRegistering = false;
                return;
            }

            var passwordRegex = new Regex(@"^(?=.*\w+)(?=.*\W*)(?=.*\d*)(?=.*\s*).{6,}$");
            if (!passwordRegex.IsMatch(Senha))
            {
                MessagingCenter.Send(this, WeakPasswordTitle);
                InRegistering = false;
                return;
            }

            if (CidadeSelecionada == null)
            {
                MessagingCenter.Send(this, CityIsntSelectedTitle);
                InRegistering = false;
                return;
            }

            var registerBinding = new RegisterBinding
            {
                Nome = Nome,
                Email = Email,
                Password = Senha,
                ConfirmPassword = ConfirmaSenha,
                CidadeId = CidadeSelecionada.Id
            };

            var result = await _authService.Register(registerBinding);

            if (result == RegisterResult.RegisterSuccefully)
            {
                MessagingCenter.Send(this, RegisterSucessfullTitle);
                InRegistering = false;
                await Navigation.PopAsync(true);
            }
            else
            {
                MessagingCenter.Send(this, RegisterUnsuccessTitle);
                InRegistering = false;
            }
        }

        public void CarregarEstados()
        {
            Estados = _realm.All<Estado>().OrderBy(x => x.Nome).ToList();
            IsEstadosCarregados = true;
        }

        public void AtualizarCidades()
        {
            Cidades =
                _realm.All<Cidade>().Where(x => x.EstadoId.Equals(EstadoSelecionado.Id)).OrderBy(x => x.Nome).ToList();
            IsCidadesCarregadas = true;
        }

        public async void Voltar()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PopAsync();
                IsBusy = false;
            }
        }
    }
}