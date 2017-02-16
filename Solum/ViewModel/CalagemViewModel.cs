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
    public class CalagemViewModel : BaseViewModel
    {
        public CalagemViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _analise = _realm.Find<Analise>(analiseId);
            PageTitle = _analise.Identificacao;

            _v2Selected = _analise.V2 != 0 ? _analise.V2.ToString() : null;
            Prnt = _analise.Prnt != 0 ? $"{_analise.Prnt:N} %" : null;
            _profundidadeSelected = _analise.Profundidade != 0 ? _analise.Profundidade.ToString() : null;

            V2List = new List<DisplayItems>();

            for (var i = 30; i <= 80; i += 5)
                V2List.Add(new DisplayItems($"{i} %", i));

            ProfundidadeList = new List<DisplayItems>
            {
                new DisplayItems("5 cm", 5),
                new DisplayItems("10 cm", 10),
                new DisplayItems("20 cm", 20),
                new DisplayItems("40 cm", 40)
            };
        }

        #region Commands

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(Save));

        public ICommand SetV2ValueCommand =>
            _setV2ValueCommand ?? (_setV2ValueCommand = new Command(SetV2Value));

        public ICommand SetProfundidadeValueCommand
            => _setProfundidadeValueCommand ?? (_setProfundidadeValueCommand = new Command(SetProfundidadeValue));

        #endregion

        #region Functions

        private async void Save()
        {
            float fPrnt, fV2, profundidade;
            float.TryParse(Prnt.Replace("%", "").Trim(), out fPrnt);
            float.TryParse(_v2Selected.Trim(), out fV2);
            float.TryParse(_profundidadeSelected.Trim(), out profundidade);

            if (fPrnt <= 0 || fPrnt > 100)
            {
                "O valor de PRNT deve estar entre 0 e 100".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            using (var transaction = _realm.BeginWrite())
            {
                _analise.Prnt = fPrnt;
                _analise.V2 = fV2;
                _analise.Profundidade = int.Parse(profundidade.ToString());
                transaction.Commit();
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                var current = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(new RecomendaCalagemPage(_analise.Id));
                Navigation.RemovePage(current);
                IsBusy = false;
            }
        }

        private void SetV2Value()
        {
            _v2Selected = V2Item.Value.ToString("N");
        }

        private void SetProfundidadeValue()
        {
            _profundidadeSelected = ((int) ProfundidadeItem.Value).ToString("N");
        }

        #endregion

        #region Private properties

        private string _v2Selected;
        private string _profundidadeSelected;
        private string _prnt;

        private DisplayItems _v2Item;
        private DisplayItems _profundidadeItem;

        private IList<DisplayItems> _v2List;
        private IList<DisplayItems> _profundidadeList;

        private ICommand _saveCommand;
        private ICommand _setV2ValueCommand;
        private ICommand _setProfundidadeValueCommand;

        private readonly Analise _analise;
        private readonly Realm _realm;

        #endregion

        #region Binding Properties

        public DisplayItems ProfundidadeItem
        {
            get { return _profundidadeItem; }
            set { SetPropertyChanged(ref _profundidadeItem, value); }
        }

        public DisplayItems V2Item
        {
            get { return _v2Item; }
            set { SetPropertyChanged(ref _v2Item, value); }
        }

        public IList<DisplayItems> ProfundidadeList
        {
            get { return _profundidadeList; }
            set { SetPropertyChanged(ref _profundidadeList, value); }
        }

        public IList<DisplayItems> V2List
        {
            get { return _v2List; }
            set { SetPropertyChanged(ref _v2List, value); }
        }


        public string Prnt
        {
            get { return _prnt; }
            set { SetPropertyChanged(ref _prnt, value); }
        }

        #endregion
    }
}