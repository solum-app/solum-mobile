using System;
using System.Linq;
using Realms;
using Solum.Interfaces;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class CoberturaViewModel : BaseViewModel
    {
        private string _cultura;

        private string _expectativa;
        private string _k20;

        private string _n;
        private string _p205;
        private string _pageTitle;


        public CoberturaViewModel(INavigation navigation, string analiseid) : base(navigation)
        {
            var realm = Realm.GetInstance();
            var analise = realm.Find<Analise>(analiseid);
            PageTitle = $"{analise.Talhao.Fazenda.Nome} - {analise.Talhao.Nome}";
            var semeadura = realm.All<Semeadura>().FirstOrDefault(s => s.AnaliseId.Equals(analiseid));
            Cultura = semeadura?.Cultura;
            Expectativa = semeadura?.Expectativa.ToString();
            var calculator = CoberturaInjector.GetInstance(semeadura.Cultura.ToUpper());
            N = calculator?.CalculateN(semeadura.Expectativa);
            P205 = calculator?.CalculateP(semeadura.Expectativa);
            K20 = calculator?.CalculateK(semeadura.Expectativa);
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

        public string P205
        {
            get { return _p205; }
            set { SetPropertyChanged(ref _p205, value); }
        }

        public string K20
        {
            get { return _k20; }
            set { SetPropertyChanged(ref _k20, value); }
        }
    }
}