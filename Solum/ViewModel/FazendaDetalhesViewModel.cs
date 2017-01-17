using System.Collections;
using System.Collections.Generic;
using Realms;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaDetalhesViewModel : BaseViewModel
    {
        private Fazenda _fazenda;
        private IEnumerable _talhoesList;
        private bool _hasItems;
        private Realm _realm;
        public FazendaDetalhesViewModel(INavigation navigation, Fazenda item) : base(navigation)
        {
            //            _realm = Realm.GetInstance();
            _fazenda = item;
            HasItems = false;
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

        public IEnumerable TalhoesList
        {
            get { return _talhoesList; }
            set { SetPropertyChanged(ref _talhoesList, value); }
        }
    }
}