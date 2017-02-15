using System.Collections.Generic;
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
            Analises = _realm.All<Analise>().OrderBy(a => a.Identificacao).ToList();
            HasItems = Analises.Any();
        }

        #region Private properties

        private bool _hasItems;
        private readonly Realm _realm;

        private ICommand _editCommand;
        private ICommand _deleteCommand;
        private ICommand _itemTappedCommand;

        private IList<Analise> _analises;

        #endregion

        #region Binding properites

        public IList<Analise> Analises
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

        #region Binding Properties

        #endregion

        #region Commands

        public ICommand EditCommand => _editCommand ?? (_editCommand = new Command(ShowEditAnalisePage));

        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new Command(DeleteAnalise));

        public ICommand ItemTappedCommand
            => _itemTappedCommand ?? (_itemTappedCommand = new Command(ShowGerenciamentoAnalisePage));

        #endregion

        #region Functions

        public void UpdateAnalisesList()
        {
            Analises = _realm.All<Analise>().OrderBy(a => a.Identificacao).ToList();
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
            Analises.Remove(analise);
            UpdateAnalisesList();
        }

        private async void ShowEditAnalisePage(object obj)
        {
            await Navigation.PushAsync(new AnalisePage(obj as Analise));
        }

        private async void ShowGerenciamentoAnalisePage(object obj)
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new GerenciamentoAnalisePage(obj as Analise));
                IsBusy = false;
            }
        }

        #endregion
    }
}