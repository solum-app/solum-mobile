using System;
using System.Linq;
using Realms;
using Solum.Interfaces;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AdubacaoCoberturaViewModel : BaseViewModel
    {

        #region Private properites

        private string _pageTitle;
        private string _expectativa;
        private string _cultura;

        private string _n;
        private string _p2O5;
        private string _k2O;

        #endregion

        #region Binding properties

        #endregion

        #region Commands

        #endregion

        #region Functions

        #endregion


        public AdubacaoCoberturaViewModel(INavigation navigation, string analiseid) : base(navigation)
        {
            var realm = Realm.GetInstance();
            var analise = realm.Find<Analise>(analiseid);
            PageTitle = analise.Identificacao;

            var semeadura = realm.All<Semeadura>().FirstOrDefault(s => s.AnaliseId.Equals(analiseid));

            Cultura = semeadura.Cultura;
			Expectativa = semeadura.Expectativa.ToString();
            var calculator = CoberturaInjector.GetInstance(semeadura.Cultura.ToUpper());
            N = calculator?.CalculateN(semeadura.Expectativa);
            P2O5 = calculator?.CalculateP(semeadura.Expectativa);
            K2O = calculator?.CalculateK(semeadura.Expectativa);
            using (var transaction = realm.BeginWrite())
            {
                analise.DataCalculoCobertura = DateTimeOffset.Now;
                transaction.Commit();
            }
        }

        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetPropertyChanged(ref _pageTitle, value); }
        }

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
    }
}