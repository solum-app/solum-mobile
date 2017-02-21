using System;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class RecomendacaoSemeaduraViewModel : BaseViewModel
    {
        public RecomendacaoSemeaduraViewModel(INavigation navigation, string analiseId, int expectativa, string cultura, bool enableButton) : base(navigation)
        {
            _realm = Realm.GetInstance();
            var interpreter = SemeaduraInjector.GetInstance(cultura);
            _analise = _realm.Find<Analise>(analiseId);
            PageTitle = _analise.Identificacao;
            Expectativa = expectativa.ToString();
            Cultura = cultura;
            N = interpreter.CalculateN(expectativa, null).ToString();
            P205 =
                interpreter.CalculateP(expectativa,
                    InterpretaHandler.InterpretaP(_analise.Fosforo,
                        InterpretaHandler.InterpretaTextura(_analise.Argila, _analise.Silte))).ToString();
            K20 =
                interpreter.CalculateK(expectativa,
                        InterpretaHandler.InterpretaK(_analise.Potassio, _analise.CTC))
                    .ToString();
            EnableButton = enableButton;
        }
        
        #region Binding Properties

        public bool EnableButton
        {
            get { return _enableButton; }
            set { SetPropertyChanged(ref _enableButton, value); }
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

        #endregion

        #region Private Properties

        private bool _enableButton;
        private string _n;
        private string _p205;
        private string _k20;

        private string _expectativa;
        private string _cultura;

        private ICommand _showSemeaduraPageCommand;

        private readonly Analise _analise;
        private readonly Realm _realm;

        #endregion

        #region Commands

        public ICommand SalvarCommand
            => _showSemeaduraPageCommand ?? (_showSemeaduraPageCommand = new Command(Salvar));

        #endregion

        #region Functions

        private async void Salvar()
        {
            using (var transaction = _realm.BeginWrite())
            {
                _analise.DataCalculoSemeadura = DateTimeOffset.Now;
                _analise.HasSemeadura = true;
                _analise.Cultura = Cultura;
                _analise.Expectativa = int.Parse(Expectativa);
                transaction.Commit();
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PopAsync();
                IsBusy = false;
            }
        }

        #endregion
    }
}