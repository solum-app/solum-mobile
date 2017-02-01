using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
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
            if(_realm.All<Calagem>().Any(c => c.AnaliseId.Equals(_analise.Id)))
                _calagem = _realm.All<Calagem>().FirstOrDefault(c => c.AnaliseId.Equals(_analise.Id));
            PageTitle = $"{_analise.Talhao.Fazenda.Nome} - {_analise.Talhao.Nome}";

            V2List = new List<int>();
            for (int i = 30; i <= 100; i += 5)
            {
                V2List.Add(i);
            }

            ProfundidadeList = new List<int>();
            ProfundidadeList.Add(5);
            ProfundidadeList.Add(10);
            ProfundidadeList.Add(20);
            ProfundidadeList.Add(40);
            ProfundidadeSelected = ProfundidadeList[1];
            V2Selected = V2List[0];
        }

        #region Private properties

        private ICommand _saveCommand;

        private string _pageTitle;

        private IList<int> _v2List;
        private int _v2Selected;

        private IList<int> _profundidadeList;
        private int _profundidadeSelected;

        private float _prnt;

        private readonly Analise _analise;
        private Calagem _calagem;
        private readonly Realm _realm;

        #endregion

        #region Binding Properties

        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetPropertyChanged(ref _pageTitle, value); }
        }
        
        public IList<int> V2List
        {
            get { return _v2List; }
            set { SetPropertyChanged(ref _v2List, value); }
        }

        public int V2Selected
        {
            get { return _v2Selected; }
            set { SetPropertyChanged(ref _v2Selected, value); }
        }

        public IList<int> ProfundidadeList
        {
            get { return _profundidadeList; }
            set { SetPropertyChanged(ref _profundidadeList, value); }
        }

        public int ProfundidadeSelected
        {
            get { return _profundidadeSelected; }
            set { SetPropertyChanged(ref _profundidadeSelected, value); }
        }

        public float Prnt
        {
            get { return _prnt; }
            set { SetPropertyChanged(ref _prnt, value); }
        }

        #endregion

        #region Commands

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(Save));

        #endregion

        #region Functions

        private async void Save()
        {
            if (_calagem == null || _calagem == default(Calagem))
            {
                _calagem = new Calagem()
                {
                    Id = Guid.NewGuid().ToString(),
                    AnaliseId = _analise.Id,
                    Analise = _analise,
                    Prnt = Prnt,
                    Profundidade = ProfundidadeSelected,
                    V2 = V2Selected
                };

                using (var transaction = _realm.BeginWrite())
                {
                    _realm.Add(_calagem);
                    _analise.DataCalculoCalagem = DateTimeOffset.Now;
                    transaction.Commit();
                }
            }
            else
            {
                using (var transaction = _realm.BeginWrite())
                {
                    _calagem.Prnt = Prnt;
                    _calagem.Profundidade = ProfundidadeSelected;
                    _calagem.V2 = V2Selected;
                    _analise.DataCalculoCalagem = DateTimeOffset.Now;
                    transaction.Commit();
                }
            }

            if (!IsBusy)
            {
                IsBusy = true;
                var current = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(new RecomendaCalagemPage(Navigation, _calagem.Id));
                Navigation.RemovePage(current);
                IsBusy = false;
            }
        }

        #endregion
    }
}