using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaDetalhesViewModel : BaseViewModel
    {
        private readonly Realm _realm;
        private ICommand _editarTalhaoCommand;
        private Fazenda _fazenda;
        private bool _hasItems;
        private ICommand _removerTalhaoCommand;
        private ICommand _itemTappedCommand;
        private IList<Talhao> _talhoesList;
        private readonly bool _fromAnalise;

        public FazendaDetalhesViewModel(INavigation navigation, Fazenda fazenda, bool fromAnalise) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Fazenda = fazenda;
            HasItems = _realm.All<Talhao>().Any(t => t.FazendaId.Equals(Fazenda.Id));
            _fromAnalise = fromAnalise;
        }

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

        public IList<Talhao> TalhoesList
        {
            get { return _talhoesList; }
            set { SetPropertyChanged(ref _talhoesList, value); }
        }

        public ICommand RemoverTalhaoCommand
            => _removerTalhaoCommand ?? (_removerTalhaoCommand = new Command(obj => ExcluirTalhao(obj as Talhao)));

        public ICommand EditarTalhaoCommand
            => _editarTalhaoCommand ?? (_editarTalhaoCommand = new Command(obj => EditarTalhao(obj as Talhao)));

        public ICommand ItemTappedCommand
            => _itemTappedCommand ?? (_itemTappedCommand = new Command(obj => Selecionar(obj)));

        private async void Selecionar(object obj)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if (_fromAnalise)
                {
                    MessagingCenter.Send(this, "TalhaoSelecionado", obj as Talhao);
                    await Navigation.PopAsync();
                }
                IsBusy = false;
            }
        }

        private async void EditarTalhao(Talhao talhao)
        {
            await Navigation.PushAsync(new TalhaoCadastroPage(talhao));
        }

        private void ExcluirTalhao(Talhao talhao)
        {
            var find = _realm.Find<Talhao>(talhao.Id);
            using (var transaction = _realm.BeginWrite())
            {
                _realm.Remove(find);
                transaction.Commit();
            }
            UpdateTalhoesList();
        }


        public void UpdateTalhoesList()
        {
            TalhoesList = _realm.All<Talhao>()
                .Where(t => t.FazendaId.Equals(Fazenda.Id))
                .OrderBy(t => t.Nome).ToList();
            HasItems = TalhoesList.Any();
        }
    }
}