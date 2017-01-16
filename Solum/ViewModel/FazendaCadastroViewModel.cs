using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaCadastroViewModel : BaseViewModel
    {
        private readonly Realm _realm;
        private ICommand _cadastrarFazendaCommand;
        private IList<Cidade> _cidadeList;
        private Cidade _cidadeSelected;
        private IList<Estado> _estadoList;
        private Estado _estadoSelected;
        private string _fazendaName;
        private bool _isCarregandoEstados = true;
        private bool _isEstadoSelected;
        private ICommand _updateCidadesListCommand;
        private int _estadoSelectedIndex;
        private int _cidadeSelectedIndex;
        private bool _isUpdate;
        private Fazenda _fazenda;
        public FazendaCadastroViewModel(INavigation navigation) : base(navigation)
        {
            _realm = Realm.GetInstance();
            EstadoList = _realm.All<Estado>().OrderBy(e => e.Nome).ToList();
        }

        public FazendaCadastroViewModel(INavigation navigation, Fazenda fazenda) : base(navigation)
        {
            _isUpdate = true;
            _realm = Realm.GetInstance();
            _fazenda = fazenda;
            FazendaName = fazenda.Nome;
            EstadoList = _realm.All<Estado>().OrderBy(e => e.Nome).ToList();
            EstadoSelectedIndex = EstadoList.IndexOf(fazenda.Cidade.Estado);
            EstadoSelected = fazenda.Cidade.Estado;
            IsEstadoSelected = true;
            CidadeList = _realm.All<Cidade>().Where(c => c.EstadoId.Equals(fazenda.Cidade.EstadoId)).OrderBy(c => c.Nome).ToList();
            CidadeSelectedIndex = CidadeList.IndexOf(fazenda.Cidade);
            CidadeSelected = fazenda.Cidade;
        }

        public string FazendaName
        {
            get { return _fazendaName; }
            set { SetPropertyChanged(ref _fazendaName, value); }
        }

        public int EstadoSelectedIndex
        {
            get { return _estadoSelectedIndex; }
            set { SetPropertyChanged(ref _estadoSelectedIndex, value); }
        }

        public int CidadeSelectedIndex
        {
            get { return _cidadeSelectedIndex; }
            set { SetPropertyChanged(ref _cidadeSelectedIndex, value); }
        }

        public bool IsEstadoSelected
        {
            get { return _isEstadoSelected; }
            set { SetPropertyChanged(ref _isEstadoSelected, value); }
        }

        public IList<Estado> EstadoList
        {
            get { return _estadoList; }
            set { SetPropertyChanged(ref _estadoList, value); }
        }

        public IList<Cidade> CidadeList
        {
            get { return _cidadeList; }
            set { SetPropertyChanged(ref _cidadeList, value); }
        }

        public Estado EstadoSelected
        {
            get { return _estadoSelected; }
            set
            {
                SetPropertyChanged(ref _estadoSelected, value);
                IsEstadoSelected = true;
            }
        }

        public Cidade CidadeSelected
        {
            get { return _cidadeSelected; }
            set { SetPropertyChanged(ref _cidadeSelected, value); }
        }

        public ICommand UpdateCidadesListCommand
            => _updateCidadesListCommand ?? (_updateCidadesListCommand = new Command(() => UpdateCidades()));

        public ICommand CadastrarFazendaCommand
            => _cadastrarFazendaCommand ?? (_cadastrarFazendaCommand = new Command(CadastrarFazenda));

        public async void CadastrarFazenda()
        {
            if (!_isUpdate)
            {
                if (string.IsNullOrEmpty(FazendaName))
                {
                    MessagingCenter.Send(this, "Erro", "Coloque o nome da Fazenda");
                    return;
                }

                if (CidadeSelected == default(Cidade))
                {
                    MessagingCenter.Send(this, "Erro", "Selecione uma Cidade para esta Fazenda");
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

                MessagingCenter.Send(this, "Sucesso", "Fazenda cadastrada com sucesso");
                await Navigation.PopAsync(true);
            }
            else
            {
                if (string.IsNullOrEmpty(FazendaName))
                {
                    MessagingCenter.Send(this, "Erro", "Coloque o nome da Fazenda");
                    return;
                }

                using (var transaction = _realm.BeginWrite())
                {
                    _fazenda.Nome = FazendaName;
                    _fazenda.CidadeId = CidadeSelected.Id;
                    _fazenda.Cidade = CidadeSelected;
                    transaction.Commit();
                }

                MessagingCenter.Send(this, "Sucesso", "Fazenda atualizada com sucesso");
                await Navigation.PopAsync(true);
            }
        }

        public void CarregarEstados()
        {
            EstadoList = _realm.All<Estado>().OrderBy(x => x.Nome).ToList();
        }

        public void UpdateCidades()
        {
            CidadeList = _realm.All<Cidade>()
                .Where(x => x.EstadoId.Equals(EstadoSelected.Id))
                .OrderBy(n => n.Nome).ToList();
        }
    }
}