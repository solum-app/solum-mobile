using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaListViewModel : BaseViewModel
    {
        private readonly Realm _realmInstance;
        private ICommand _editarCommand;
        private ICommand _excluirCommand;
        private ICommand _itemTappedCommand;
        private IList<Fazenda> _fazendas;
        private bool _hasItems;

        public FazendaListViewModel(INavigation navigation) : base(navigation)
        {
            _realmInstance = Realm.GetInstance();
            HasItems = _realmInstance.All<Fazenda>().Any();
            if (HasItems)
                Fazendas = _realmInstance.All<Fazenda>().OrderBy(x => x.Nome).ToList();
        }

        public IList<Fazenda> Fazendas
        {
            get { return _fazendas; }
            set { SetPropertyChanged(ref _fazendas, value); }
        }

        public bool HasItems
        {
            get { return _hasItems; }
            set { SetPropertyChanged(ref _hasItems, value); }
        }

        public ICommand EditarCommand => _editarCommand ?? (_editarCommand = new Command(obj => Editar(obj)));

        public ICommand ExcluirCommand => _excluirCommand ?? (_excluirCommand = new Command(obj => Excluir(obj)));

        public ICommand ItemTappedCommand
            => _itemTappedCommand ?? (_itemTappedCommand = new Command(obj => Detalhes(obj)));

        private void Excluir(object obj)
        {
            var fazenda = obj as Fazenda;
            using (var tsc = _realmInstance.BeginWrite())
            {
                _realmInstance.Remove(fazenda);
                tsc.Commit();
            }
            UpdateFazendaList();
        }

        private async void Detalhes(object obj)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new FazendaDetalhesPage(obj as Fazenda));
                IsBusy = false;
            }
        }
        
        private async void Editar(object obj)
        {
            await Navigation.PushAsync(new FazendaCadastroPage(obj as Fazenda));
        }

        public void UpdateFazendaList()
        {
            Fazendas = _realmInstance.All<Fazenda>().OrderBy(x => x.Nome).ToList();
            HasItems = Fazendas.Any();
        }
    }
}