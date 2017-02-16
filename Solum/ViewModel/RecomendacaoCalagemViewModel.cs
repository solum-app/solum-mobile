using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class RecomendacaoCalagemViewModel : BaseViewModel
    {
        public RecomendacaoCalagemViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _analise = _realm.Find<Analise>(analiseId);
            PageTitle = _analise.Identificacao;
            V2 = _analise.V2.ToString("N");
            Prnt = _analise.Prnt.ToString("N");
            Profundidade = _analise.Profundidade.ToString("N");
            Calculate();

            using (var tnsc = _realm.BeginWrite())
            {
                _analise.V2 = 0;
                _analise.Prnt = 0;
                _analise.Profundidade = 0;
                tnsc.Commit();
            }
        }

        #region Commands

        public ICommand ShowCalagemPageCommand
            => _showCalagemPageCommand ?? (_showCalagemPageCommand = new Command(ShowCalagemPage));

        #endregion

        #region private properties
        private string _quantidade;
        private string _v2;
        private string _prnt;
        private string _profundidade;

        private readonly Analise _analise;
        private readonly Realm _realm;

        private readonly IDictionary<int, float> _values = new Dictionary<int, float>
        {
            {5, 0.25f},
            {10, 0.5f},
            {20, 1.0f},
            {40, 2.0f}
        };

        private ICommand _showCalagemPageCommand;

        #endregion

        #region Binding Properties

        public string QuantidadeCal
        {
            get { return _quantidade; }
            set { SetPropertyChanged(ref _quantidade, value); }
        }

        public string V2
        {
            get { return _v2; }
            set { SetPropertyChanged(ref _v2, value); }
        }

        public string Prnt
        {
            get { return _prnt; }
            set { SetPropertyChanged(ref _prnt, value); }
        }

        public string Profundidade
        {
            get { return _profundidade; }
            set { SetPropertyChanged(ref _profundidade, value); }
        }

        #endregion

        #region Functions

        private async void ShowCalagemPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                var current = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(new CalagemPage(_analise.Id));
                Navigation.RemovePage(current);
                IsBusy = false;
            }
        }

        public void Calculate()
        {
            float fPrnt, fV2;
            float.TryParse(Prnt, out fPrnt);
            float.TryParse(V2, out fV2);
            var f = 100f / fPrnt;
            var ctc = _analise.CTC;
            if(_values.Keys.Contains(_analise.Profundidade))
                QuantidadeCal = ((fV2 - _analise.V) * ctc / 100f * f * _values[_analise.Profundidade]).ToString("N");
            QuantidadeCal = ((fV2 - _analise.V) * ctc / 100f * f * _values[20]).ToString("N");
        }

        #endregion
    }
}