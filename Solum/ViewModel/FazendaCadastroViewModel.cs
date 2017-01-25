using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Messages;
using Solum.Models;
using Xamarin.Forms;
using static Solum.Messages.FazendaMessages;

namespace Solum.ViewModel
{
    public class FazendaCadastroViewModel : BaseViewModel
    {
        public FazendaCadastroViewModel(INavigation navigation, bool fromAnalise) : base(navigation)
        {
            _realm = Realm.GetInstance();
            LoadEstados();
            _fromAnalise = fromAnalise;
        }

        public FazendaCadastroViewModel(INavigation navigation, Fazenda fazenda) : base(navigation)
        {
            _isUpdate = true;
            _realm = Realm.GetInstance();
            _fazenda = fazenda;
            FazendaName = fazenda.Nome;
            LoadEstados();
            EstadoSelected = _realm.Find<Estado>(_fazenda.Cidade.EstadoId);
            LoadCidades();
            CidadeSelected = _realm.Find<Cidade>(_fazenda.CidadeId);
        }

        public FazendaCadastroViewModel(INavigation navigation, Fazenda fazenda, bool fromAnalise) : base(navigation)
        {
            _isUpdate = true;
            _fromAnalise = fromAnalise;
            _realm = Realm.GetInstance();
            _fazenda = fazenda;
            FazendaName = fazenda.Nome;
            LoadEstados();
            EstadoSelected = _realm.Find<Estado>(_fazenda.Cidade.EstadoId);
            LoadCidades();
            CidadeSelected = _realm.Find<Cidade>(_fazenda.CidadeId);
        }

        #region Propriedades Privadas

        private ICommand _registerFazendaCommand;
        private ICommand _updateCidadesCommand;

        private IList<Cidade> _cidades;
        private IList<Estado> _estados;
        private Cidade _cidadeSelected;
        private Estado _estadoSelected;

        private string _fazendaName;
        private bool _isEstadosLoaded;
        private bool _isCidadesLoaded;
        private readonly bool _isUpdate;

        private readonly Fazenda _fazenda;
        private readonly Realm _realm;

        private readonly bool _fromAnalise;

        #endregion

        #region Propriedade de Binding

        public string FazendaName
        {
            get { return _fazendaName; }
            set { SetPropertyChanged(ref _fazendaName, value); }
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

        #endregion

        #region Comandos

        public ICommand UpdateCidadesCommand
            => _updateCidadesCommand ?? (_updateCidadesCommand = new Command(LoadCidades));

        public ICommand RegisterFazendaCommand
            => _registerFazendaCommand ?? (_registerFazendaCommand = new Command(RegisterFazenda));

        #endregion

        #region Funções

        public void LoadEstados()
        {
            Estados = _realm.All<Estado>().OrderBy(x => x.Nome).ToList();
            IsEstadosLoaded = true;
        }

        public void LoadCidades()
        {
            Cidades = _realm.All<Cidade>()
                .Where(x => x.EstadoId.Equals(EstadoSelected.Id))
                .OrderBy(n => n.Nome)
                .ToList();
            IsCidadesLoaded = true;
        }

        public async void RegisterFazenda()
        {
            if (!_isUpdate)
            {
                if (string.IsNullOrEmpty(FazendaName))
                {
                    FazendaNameNull.ToDisplayAlert(MessageType.Erro);
                    return;
                }

                if (CidadeSelected == null)
                {
                    CidadeIsntSelected.ToDisplayAlert(MessageType.Erro);
                    return;
                }

                var usuario = _realm.All<Usuario>().FirstOrDefault();

                var fazenda = new Fazenda
                {
                    Id = Guid.NewGuid().ToString(),
                    Nome = FazendaName,
                    CidadeId = CidadeSelected.Id,
                    Cidade = CidadeSelected,
                    UsuarioId = usuario.Id,
                    Usuario = usuario
                };

                using (var transaction = _realm.BeginWrite())
                {
                    _realm.Add(fazenda);
                    transaction.Commit();
                }

                if (!_fromAnalise)
                    Success.ToToast(ToastNotificationType.Sucesso);
                else
                    MessagingCenter.Send(this, MessagingCenterMessages.FazendaSelected, fazenda.Id);
            }
            else
            {
                if (string.IsNullOrEmpty(FazendaName))
                {
                    FazendaNameNull.ToDisplayAlert(MessageType.Erro);
                    return;
                }

                using (var transaction = _realm.BeginWrite())
                {
                    _fazenda.Nome = FazendaName;
                    _fazenda.Cidade = CidadeSelected;
                    _fazenda.CidadeId = CidadeSelected.Id;
                    transaction.Commit();
                }
                MessagingCenter.Send(this, MessagingCenterMessages.FazendaSelected, _fazenda.Id);
                Updated.ToToast();
            }
            await Navigation.PopAsync();
        }
        #endregion
    }
}