using System;
using System.Linq;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class CorretivaViewModel : BaseViewModel
    {
        public CorretivaViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _analise = _realm.Find<Analise>(analiseId);
            PageTitle = $"{_analise.Talhao.Fazenda.Nome} - {_analise.Talhao.Nome}".ToUpper();
            Calculate();
        }

        #region Private Properties

        private string _pageTitle;

        private string _p2O5;
        private string _k2O;

        private Analise _analise;
        private readonly Realm _realm;

        #endregion

        #region Binding Properties

        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetPropertyChanged(ref _pageTitle, value); }
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

        #region Functions

        private void Calculate()
        {
            var argila = _analise.Argila;
            var pInterpretaded = InterpretaHandler.InterpretaP(_analise.Fosforo, InterpretaHandler.InterpretaTextura(_analise.Argila, _analise.Silte)).ToUpper();
            var kInterpretaded = InterpretaHandler.InterpretaK(_analise.Potassio, _analise.CTC).ToUpper();

            if (!pInterpretaded.Equals("Adequado".ToUpper()) || !pInterpretaded.Equals("Alto".ToUpper()))
            {
                if (pInterpretaded.Equals("Muito Baixo".ToUpper()))
                    P2O5 = ((argila / 10) * 4).ToString("N");
                else if (pInterpretaded.Equals("Baixo".ToUpper()))
                    P2O5 = ((argila / 10) * 2).ToString("N");
                else if (pInterpretaded.Equals("Medio".ToUpper()))
                    P2O5 = ((argila / 10) * 1).ToString("N");
            }
            else
                P2O5 = pInterpretaded;

            if (!kInterpretaded.Equals("Adequado".ToUpper()) || !kInterpretaded.Equals("Alto".ToUpper()))
            {
                var ctc = _analise.CTC;
                if (ctc < 4)
                {
                    if (kInterpretaded.Equals("Baixo".ToUpper()))
                        K2O = 50.0f.ToString("N");
                    else if (kInterpretaded.Equals("Medio".ToUpper()))
                        K2O = 25.0f.ToString("N");
                }
                else
                {
                    if (kInterpretaded.Equals("Baixo".ToUpper()))
                        K2O = 100.0f.ToString("N");
                    else if (kInterpretaded.Equals("Medio".ToUpper()))
                        K2O = 50.0f.ToString("N");
                }
            }
            else
                K2O = kInterpretaded;

            using (var transaction = _realm.BeginWrite())
            {
                _analise.DataCalculoCorretiva = DateTimeOffset.Now;
                transaction.Commit();
            }
        }

        #endregion
    }
}