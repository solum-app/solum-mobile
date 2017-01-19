using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
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
        private ICommand _registrarCommand;
        private ICommand _atualizarCidadesCommand;
        private ICommand _voltarCommand;

        private IList<Cidade> _cidades;
        private IList<Estado> _estados;
        private Cidade _cidadeSelecionada;
        private Estado _estadoSelecionado;

        private bool _isEstadosCarregados;
        private bool _isCidadesCarregadas;
        private bool _inRegistering;

        private string _nome;
        private string _senha;
        private string _confirmaSenha;
        private string _email;

        private readonly Realm _realm;
        private readonly AuthService _authService;

        public CadastroViewModel(INavigation navigation) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _authService = AuthService.Instance;
            IsEstadosCarregados = false;
            CarregarEstados();
        }

        public ICommand RegistrarCommand => _registrarCommand ?? (_registrarCommand = new Command(Cadastrar));

        public ICommand AtualizarCidadesCommand => _atualizarCidadesCommand ?? (_atualizarCidadesCommand = new Command(AtualizarCidades));

        public ICommand VoltarCommand => _voltarCommand ?? (_voltarCommand = new Command(Voltar));

        public string Nome
        {
            get { return _nome; }
            set { SetPropertyChanged(ref _nome, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetPropertyChanged(ref _email, value); }
        }

        public string Senha
        {
            get { return _senha; }
            set { SetPropertyChanged(ref _senha, value); }
        }

        public string ConfirmaSenha
        {
            get { return _confirmaSenha; }
            set { SetPropertyChanged(ref _confirmaSenha, value); }
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
            var registerBinding = new RegisterBinding
            {
                Nome = Nome,
                Email = Email,
                Password = Senha,
                ConfirmPassword = ConfirmaSenha
            };

            if (!registerBinding.IsValid)
            {
                MessagingCenter.Send(this, EntryNullValuesTitle, EntryNullValuesMessage);
                InRegistering = false;
                return;
            }

            if (CidadeSelecionada == default(Cidade))
            {
                MessagingCenter.Send(this, EntryNullValuesTitle, EntryNullValuesMessage);
                InRegistering = false;
                return;
            }

            registerBinding.CidadeId = CidadeSelecionada.Id;

            var result = await _authService.Register(registerBinding);

            if (result == RegisterResult.RegisterSuccefully)
            {
                MessagingCenter.Send(this, RegisterSucessfullTitle, RegisterSuccessfullMessage);
                InRegistering = false;
                await Navigation.PopAsync(true);
            }
            else { 
                MessagingCenter.Send(this, RegisterUnsuccessTitle, RegisterUnsuccessMessage);
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
            Cidades = _realm.All<Cidade>().Where(x => x.EstadoId.Equals(EstadoSelecionado.Id)).OrderBy(x => x.Nome).ToList();
            IsCidadesCarregadas = true;
        }

        public async void Voltar()
        {
            await Navigation.PopAsync();
        }
    }
}