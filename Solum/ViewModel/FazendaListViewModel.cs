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
    public class FazendaListViewModel : BaseViewModel
    {
        public FazendaListViewModel(INavigation navigation, bool fromAnalise) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Fazendas = _realm.All<Fazenda>().OrderBy(x => x.Nome).ToList();
            HasItems = Fazendas.Any();
            _fromAnalise = fromAnalise;
        }

        #region Propriedades privadas

        private ICommand _editCommand;
        private ICommand _deleteCommand;
        private ICommand _itemTappedCommand;

        private IList<Fazenda> _fazendas;
        private bool _hasItems;
        private readonly bool _fromAnalise;

        private readonly Realm _realm;

        #endregion

        #region Propriedades de Binding

        public bool HasItems
        {
            get { return _hasItems; }
            set { SetPropertyChanged(ref _hasItems, value); }
        }

        public IList<Fazenda> Fazendas
        {
            get { return _fazendas; }
            set { SetPropertyChanged(ref _fazendas, value); }
        }

        #endregion

        #region Comandos

        public ICommand EditCommand => _editCommand ?? (_editCommand = new Command(fazenda => Edit(fazenda as Fazenda)))
        ;

        public ICommand DeleteCommand
            => _deleteCommand ?? (_deleteCommand = new Command(fazenda => Delete(fazenda as Fazenda)));

        public ICommand ItemTappedCommand
            => _itemTappedCommand ?? (_itemTappedCommand = new Command(fazenda => Details(fazenda as Fazenda)));

        #endregion

        #region Funções

        private async void Details(Fazenda fazenda)
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                if (_fromAnalise)
                {
                    MessagingCenter.Send(this, MessagingCenterMessages.FazendaSelected, fazenda.Id);
                    await Navigation.PopAsync();
                }
                else
                {
                    await Navigation.PushAsync(new FazendaDetalhesPage(fazenda.Id, _fromAnalise));
                }
                IsBusy = false;
            }
        }

        private async void Edit(Fazenda fazenda)
        {
            var current = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(new FazendaCadastroPage(fazenda.Id, _fromAnalise));
            if (_fromAnalise)
                Navigation.RemovePage(current);
        }

        private void Delete(Fazenda fazenda)
        {
            using (var transaction = _realm.BeginWrite())
            {
                _realm.Remove(fazenda);
                transaction.Commit();
            }
            FazendaMessages.Deleted.ToToast();
            UpdateFazendaList();
        }

        public bool CanDelete(string fazendaId)
        {
            var talhoes = _realm.All<Talhao>().Where(t => t.FazendaId.Equals(fazendaId)).ToList();
            foreach (var t in talhoes)
            {
                if (_realm.All<Analise>().Any(a => a.TalhaoId.Equals(t.Id)))
                    return false;
            }
            return true;
        }
        public void UpdateFazendaList()
        {
            Fazendas = _realm.All<Fazenda>().OrderBy(x => x.Nome).ToList();
            HasItems = Fazendas.Any();
        }

        #endregion
    }
}