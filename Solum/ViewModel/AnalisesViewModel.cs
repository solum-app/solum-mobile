using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AnalisesViewModel : BaseViewModel
    {
        public AnalisesViewModel(INavigation navigation) : base(navigation)
        {
            _realm = Realm.GetInstance();
            analises = _realm.All<Analise>().OrderBy(e => e.Talhao.Fazenda.Nome).ToList();

            var groupList = analises.GroupBy(a => a.Talhao.Fazenda.Nome.ToUpper())
                .Select(a => new Grouping<string, Analise>(a.Key, a));

            Analises = new ObservableCollection<Grouping<string, Analise>>(groupList);
            HasItems = Analises.Any();
        }

        #region Private Properties

        private ICommand _editCommand;
        private ICommand _deleteCommand;
        private ICommand _itemTappedCommand;

        private IList<Analise> analises;
        private IList<Grouping<string, Analise>> _analises;

        private bool _hasItems;

        private readonly Realm _realm;

        #endregion

        #region Binding Properties

        public IList<Grouping<string, Analise>> Analises
        {
            get { return _analises; }
            set { SetPropertyChanged(ref _analises, value); }
        }

        public bool HasItems
        {
            get { return _hasItems; }
            set { SetPropertyChanged(ref _hasItems, value); }
        }

        #endregion

        #region Commands

        public ICommand EditCommand => _editCommand ?? (_editCommand = new Command(ShowEditAnalisePage));

        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(DeleteAnalise));

        public ICommand ItemTappedCommand => _itemTappedCommand ?? (_itemTappedCommand = new Command(ShowGerenciamentoAnalisePage));

        #endregion

        #region Functions

        private void UpdateAnalises()
        {
            analises = _realm.All<Analise>().OrderBy(e => e.Talhao.Fazenda.Nome).ToList();

            var groupList = analises.GroupBy(a => a.Talhao.Fazenda.Nome.ToUpper())
                .Select(a => new Grouping<string, Analise>(a.Key, a));

            Analises = new ObservableCollection<Grouping<string, Analise>>(groupList);
            HasItems = Analises.Any();
        }

        private void DeleteAnalise(object obj)
        {
            var analise = obj as Analise;
            using (var transaction = _realm.BeginWrite())
            {
                _realm.Remove(analise);
                transaction.Commit();
            }

            analises.Remove(analise);
            UpdateAnalises();
        }

        private async void ShowEditAnalisePage(object obj)
        {
            await Navigation.PushAsync(new AnalisePage(obj as Analise));
        }

        private async void ShowGerenciamentoAnalisePage(object obj)
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new GerenciamentoAnalisePage(obj as Analise));
                IsBusy = false;
            }
        }

        #endregion
    }
}