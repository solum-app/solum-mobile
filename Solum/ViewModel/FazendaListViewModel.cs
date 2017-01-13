using System.Collections.Generic;
using System.Windows.Input;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaListViewModel : BaseViewModel
    {
        private IList<Fazenda> _fazendas;
        private bool _hasItems;
        private ICommand _itemTappedCommand;
        private ICommand _editarCommand;
        private ICommand _excluirCommand;
        private FazendaDataService _service;

        public FazendaListViewModel(INavigation navigation) : base(navigation)
        {
            _service = new FazendaDataService();
            Fazendas = _service.Find();
            HasItems = Fazendas.Count > 0;
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

        public ICommand EditarCommand => _editarCommand ?? (_editarCommand = new Command((obj) => Editar(obj)));

        public ICommand ExcluirCommand => _excluirCommand ?? (_excluirCommand = new Command((obj) => Excluir(obj)));

        public ICommand ItemTappedCommand => _itemTappedCommand ?? (_itemTappedCommand = new Command(obj => FazendaDetail(obj)));

        private void Excluir(object obj)
        {
            var fazenda = obj as Fazenda;
            _service.Delete(fazenda);
            _fazendas.Remove(fazenda);
            Fazendas = _fazendas;
        }

        private async void FazendaDetail(object obj)
        {
            await Navigation.PushAsync(new FazendaDetalhesPage(obj as Fazenda));
        }


        private async void Editar(object obj)
        {
            await Navigation.PushAsync(new FazendaCadastroPage(obj as Fazenda));
        }
    }
}