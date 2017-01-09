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
        private string _aviso;
        private IList<Cidade> _cidades = new List<Cidade>();

        private Cidade _cidadeSelected;
        private string _confirmPassword;
        private string _email;
        private IList<Estado> _estados = new List<Estado>();
        private Estado _estadoSelected;
        private bool _isAvisoVisible;

        private bool _isCarregandoEstados = true;
        private bool _isCidadesCarregadas;

        private string _name;
        private string _password;

        private ICommand _registerCommand;
        private ICommand _updateCidadesCommand;

        public CadastroViewModel(INavigation navigation) : base(navigation)
        {
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

        public bool IsAvisoVisible
        {
            get { return _isAvisoVisible; }
            set { SetPropertyChanged(ref _isAvisoVisible, value); }
        }

        public string Aviso
        {
            get { return _aviso; }
            set { SetPropertyChanged(ref _aviso, value); }
        }

        public ICommand RegisterCommand
        {
            get { return _registerCommand ?? (_registerCommand = new Command(Cadastrar)); }
        }

        public ICommand UpdateCidadesCommand
        {
            get { return _updateCidadesCommand ?? (_updateCidadesCommand = new Command(AtualizarCidades)); }
        }

        public async void Cadastrar()
        {
            var registerBinding = new RegisterBinding
            {
                Nome = Name,
                Email = Email,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                CidadeId = CidadeSelected.Id
            };
            var authService = new AuthService();
            var result = await authService.Register(registerBinding);
            IsAvisoVisible = true;
            if (result == RegisterResult.RegisterSuccefully)
            {
                Aviso = "Seu cadastro foi realizado com sucesso!";
            }
            if (result == RegisterResult.RegisterUnsuccessfully)
            {
                Aviso = "Seu cadastro não foi realizado com sucesso.";
            }
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            CidadeSelected = null;
            EstadoSelected = null;
        }

        public async Task CarregarEstados()
        {
            var realm = Realm.GetInstance();
            Estados = realm.All<Estado>().OrderBy(x => x.Nome).ToList();
            if (Estados == default(IList<Estado>) || Estados.Count < 27)
                await SyncService.CidadeEstadoSync();
            Estados = realm.All<Estado>().OrderBy(x => x.Nome).ToList();
            IsCarregandoEstados = false;
        }

        public void AtualizarCidades()
        {
            Cidades = EstadoSelected.Cidades.OrderBy(x => x.Nome).ToList();
            IsCidadesCarregadas = true;
        }
    }
}