using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Messages;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaDetalhesViewModel : BaseViewModel
    {
        public FazendaDetalhesViewModel(INavigation navigation, string fazendaId, bool fromAnalise) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Fazenda = _realm.Find<Fazenda>(fazendaId);
            HasItems = _realm.All<Talhao>().Any(t => t.FazendaId.Equals(Fazenda.Id));
            _fromAnalise = fromAnalise;
            PageTitle = Fazenda.Nome;
        }

        #region Propriedades privadas

        private ICommand _showEditTalhaoPageCommand;
        private ICommand _itemTappedCommand;
        private ICommand _deleteTalhaoCommand;

        private bool _hasItems;
        private readonly bool _fromAnalise;

        private IList<Talhao> _talhoes;
        private Fazenda _fazenda;

        private readonly Realm _realm;

        #endregion

        #region Propriedades de Binding

        public bool HasItems
        {
            get { return _hasItems; }
            set { SetPropertyChanged(ref _hasItems, value); }
        }

        public Fazenda Fazenda
        {
            get { return _fazenda; }
            set { SetPropertyChanged(ref _fazenda, value); }
        }

        public IList<Talhao> Talhoes
        {
            get { return _talhoes; }
            set { SetPropertyChanged(ref _talhoes, value); }
        }

        #endregion

        #region Comandos

        public ICommand DeleteTalhaoCommand
            => _deleteTalhaoCommand ?? (_deleteTalhaoCommand = new Command(obj => DeleteTalhao(obj as Talhao)));

        public ICommand ShowEditTalhaoPageCommand
            =>
                _showEditTalhaoPageCommand ??
                (_showEditTalhaoPageCommand = new Command(obj => ShowEditTalhaoPage(obj as Talhao)));

        public ICommand ItemTappedCommand
            => _itemTappedCommand ?? (_itemTappedCommand = new Command(obj => SelectTalhao(obj as Talhao)));

        #endregion

        #region Funções

        private async void SelectTalhao(Talhao talhao)
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                if (_fromAnalise)
                {
                    MessagingCenter.Send(this, MessagingCenterMessages.TalhaoSelected, talhao.Id);
                    await Navigation.PopAsync();
                }
                IsBusy = false;
            }
        }

        private async void ShowEditTalhaoPage(Talhao talhao)
        {
            await Navigation.PushAsync(new TalhaoCadastroPage(talhao.Id));
        }

        private void DeleteTalhao(Talhao talhao)
        {
            var find = _realm.Find<Talhao>(talhao.Id);
            using (var transaction = _realm.BeginWrite())
            {
                _realm.Remove(find);
                transaction.Commit();
            }
            TalhaoMessages.Deleted.ToToast();
            UpdateTalhoesList();
        }


        public void UpdateTalhoesList()
        {
            Talhoes = _realm.All<Talhao>()
                .Where(t => t.FazendaId.Equals(Fazenda.Id))
                .OrderBy(t => t.Nome).ToList();
            HasItems = Talhoes.Any();
        }

        #endregion
    }
}