using Realms;
using Solum.Interfaces;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AdubacaoCoberturaViewModel : BaseViewModel
    {
        public AdubacaoCoberturaViewModel(INavigation navigation, string analiseid) : base(navigation)
        {
            var realm = Realm.GetInstance();
            _analise = realm.Find<Analise>(analiseid);
            PageTitle = _analise.Identificacao;
            Cultura = _analise.Cultura;
            Expectativa = _analise.Expectativa.ToString();
            Calculate();
        }

        #region Functions

        private void Calculate()
        {
            var calculator = CoberturaInjector.GetInstance(_analise.Cultura.ToUpper());
            N = calculator?.CalculateN(_analise.Expectativa);
            P2O5 = calculator?.CalculateP(_analise.Expectativa);
            K2O = calculator?.CalculateK(_analise.Expectativa);
        }

        #endregion

        #region Private properites

        private string _expectativa;
        private string _cultura;

        private string _n;
        private string _p2O5;
        private string _k2O;

        private readonly Analise _analise;

        #endregion

        #region Binding properties

        public string Expectativa
        {
            get { return _expectativa; }
            set { SetPropertyChanged(ref _expectativa, value); }
        }

        public string Cultura
        {
            get { return _cultura; }
            set { SetPropertyChanged(ref _cultura, value); }
        }

        public string N
        {
            get { return _n; }
            set { SetPropertyChanged(ref _n, value); }
        }

        public string P2O5
        {
            get { return _p2O5; }
            set { SetPropertyChanged(ref _p2O5, value); }
        }

        public string K2O
        {
            get { return _k2O; }
            set { SetPropertyChanged(ref _k2O, value); }
        }

        #endregion

        #region Commands

        #endregion
    }
}