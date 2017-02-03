using System;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class RecomendacaoSemeaduraViewModel : BaseViewModel
    {
        public RecomendacaoSemeaduraViewModel(INavigation navigation, string analiseId, int expectativa, string cultura) : base(navigation)
        {
            var interpreter = SemeaduraInjector.GetInstance(cultura);
            var analise = Realm.GetInstance().Find<Analise>(analiseId);
            PageTitle = $"{analise.Talhao.Fazenda.Nome} - {analise.Talhao.Nome}";
            Expectativa = $"{expectativa} T/ha";
            Cultura = cultura;
            N = interpreter.CalculateN(expectativa, null).ToString();
            P205 =
                interpreter.CalculateP(expectativa,
                    InterpretaHandler.InterpretaP(analise.Fosforo,
                        InterpretaHandler.InterpretaTextura(analise.Argila, analise.Silte))).ToString();
            K20 =
                interpreter.CalculateK(expectativa, 
                    InterpretaHandler.InterpretaK(analise.Potassio, analise.CTC))
                .ToString();

            using (var transaction = Realm.GetInstance().BeginWrite())
            {
                analise.DataCalculoSemeadura = DateTimeOffset.Now;
                transaction.Commit();
            }
        }

        private string _n;
        private string _p205;
        private string _k20;

        private string _pageTitle;
        private string _expectativa;
        private string _cultura;

        public string PageTitle
        {
            get { return _pageTitle;}
            set { SetPropertyChanged(ref _pageTitle, value); }
        }

        public string Expectativa
        {
            get { return _expectativa;}
            set { SetPropertyChanged(ref _expectativa, value); }
        }

        public string Cultura
        {
            get { return _cultura;}
            set { SetPropertyChanged(ref _cultura, value); }
        }

        public string N
        {
            get { return _n;}
            set { SetPropertyChanged(ref _n, value); }
        }

        public string P205
        {
            get { return _p205;}
            set { SetPropertyChanged(ref _p205, value); }
        }

        public string K20
        {
            get { return _k20;}
            set { SetPropertyChanged(ref _k20, value); }
        }
    }
}