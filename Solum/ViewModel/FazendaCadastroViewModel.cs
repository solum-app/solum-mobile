using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Realms;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaCadastroViewModel : BaseViewModel
    {

        private ICommand _cadastrarFazendaCommand;
        private ICommand _atualizarListaCidadesCommand;

        private IList<Cidade> _cidadeList;
        private IList<Estado> _estadoList;
        private Cidade _cidadeSelecionada;
        private Estado _estadoSelecionado;

        private string _nomeFazenda;
        private bool _isEstadosCarregados;
        private bool _isCidadesCarregadas;
        private readonly bool _isUpdate;

        private Fazenda _fazenda;
        private readonly Realm _realm;

        public FazendaCadastroViewModel(INavigation navigation) : base(navigation)
        {
            _realm = Realm.GetInstance();
            CarregarEstados();
        }

        public FazendaCadastroViewModel(INavigation navigation, Fazenda fazenda) : base(navigation)
        {
            _isUpdate = true;
            _realm = Realm.GetInstance();
            _fazenda = fazenda;
            NomeFazenda = fazenda.Nome;
            CarregarEstados();
        }

        public string NomeFazenda
        {
            get { return _nomeFazenda; }
            set { SetPropertyChanged(ref _nomeFazenda, value); }
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

        public Estado EstadoSelecionado
        {
            get { return _estadoSelecionado; }
            set { SetPropertyChanged(ref _estadoSelecionado, value); }
        }

        public Cidade CidadeSelecionada
        {
            get { return _cidadeSelecionada; }
            set { SetPropertyChanged(ref _cidadeSelecionada, value); }
        }

        public ICommand AtualizarListaCidadesCommand
            => _atualizarListaCidadesCommand ?? (_atualizarListaCidadesCommand = new Command(UpdateCidades));

        public ICommand CadastrarFazendaCommand
            => _cadastrarFazendaCommand ?? (_cadastrarFazendaCommand = new Command(CadastrarFazenda));

        public async void CadastrarFazenda()
        {
            if (!_isUpdate)
            {
                if (string.IsNullOrEmpty(NomeFazenda))
                {
                    MessagingCenter.Send(this, "Erro", "Coloque o nome da Fazenda");
                    return;
                }

                if (CidadeSelecionada == default(Cidade))
                {
                    MessagingCenter.Send(this, "Erro", "Selecione uma Cidade para esta Fazenda");
                    return;
                }

                var usuario = _realm.All<Usuario>().FirstOrDefault();

                using (var transaction = _realm.BeginWrite())
                {
                    var fazenda = new Fazenda
                    {
                        Id = Guid.NewGuid().ToString(),
                        Nome = NomeFazenda,
                        CidadeId = CidadeSelecionada.Id,
                        Cidade = CidadeSelecionada,
                        UsuarioId = usuario.Id,
                        Usuario = usuario
                    };
                    _realm.Add(fazenda);
                    transaction.Commit();
                }
                MessagingCenter.Send(this, "Sucesso", "Fazenda cadastrada com sucesso");
                await Navigation.PopAsync(true);
            }
            else
            {
                if (string.IsNullOrEmpty(NomeFazenda))
                {
                    MessagingCenter.Send(this, "Erro", "Coloque o nome da Fazenda");
                    return;
                }

                using (var transaction = _realm.BeginWrite())
                {
                    _fazenda.Nome = NomeFazenda;
                    if (CidadeSelecionada != default(Cidade))
                    {
                        _fazenda.CidadeId = CidadeSelecionada.Id;
                        _fazenda.Cidade = CidadeSelecionada;
                    }
                    transaction.Commit();
                }
                MessagingCenter.Send(this, "Sucesso", "Fazenda atualizada com sucesso");
                await Navigation.PopAsync(true);
            }
        }

        public void CarregarEstados()
        {
            EstadoList = _realm.All<Estado>().OrderBy(x => x.Nome).ToList();
            IsEstadosCarregados = true;
        }

        public void UpdateCidades()
        {
            CidadeList = _realm.All<Cidade>()
                .Where(x => x.EstadoId.Equals(EstadoSelecionado.Id))
                .OrderBy(n => n.Nome)
                .ToList();
            IsCidadesCarregadas = true;
        }
    }
}