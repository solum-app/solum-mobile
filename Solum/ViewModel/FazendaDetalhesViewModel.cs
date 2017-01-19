using System.Collections.Generic;
using System.Linq;
using Realms;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class FazendaDetalhesViewModel : BaseViewModel
    {
        private readonly Realm _realm;
        private Fazenda _fazenda;
        private bool _hasItems;
        private IList<Talhao> _talhoesList;

        public FazendaDetalhesViewModel(INavigation navigation, Fazenda fazenda) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Fazenda = fazenda;
            HasItems = _realm.All<Talhao>().Any(t => t.FazendaId.Equals(Fazenda.Id));
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

        public void UpdateTalhoesList()
        {
            TalhoesList = _realm.All<Talhao>()
                .Where(t => t.FazendaId.Equals(Fazenda.Id))
                .OrderBy(t => t.Nome).ToList();
            HasItems = TalhoesList.Any();
        }
    }
}