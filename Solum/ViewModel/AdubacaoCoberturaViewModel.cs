using System;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AdubacaoCoberturaViewModel : BaseViewModel
    {
        public AdubacaoCoberturaViewModel(INavigation navigation, string analiseid, bool enableButton) : base(navigation)
        {
            _realm = Realm.GetInstance();
            _analise = _realm.Find<Analise>(analiseid);
            PageTitle = _analise.Identificacao;
            Cultura c;
            Enum.TryParse(_analise.Cultura, out c);
            Cultura = c;
            Expectativa = _analise.Expectativa.ToString();
            Calculate();
            EnableButton = enableButton;
        }

        #region Commands

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(Salvar));

        #endregion

        #region Functions

        private void Calculate()
        {
            var calculator = CoberturaInjector.GetInstance(Cultura);
            N = calculator?.CalculateN(_analise.Expectativa);
            P2O5 = calculator?.CalculateP(_analise.Expectativa);
            K2O = calculator?.CalculateK(_analise.Expectativa);
        }

        private async void Salvar()
        {
            using (var transaction = _realm.BeginWrite())
            {
                _analise.DataCalculoCobertura = DateTimeOffset.Now;
                _analise.HasCobertura = true;
                transaction.Commit();
            }

            MessagesResource.DadosSalvos.ToToast();

            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PopAsync();
                IsBusy = false;
            }
        }

        #endregion

        #region Private properites

        private bool _enableButton;
        private string _expectativa;
        private Cultura _cultura;

        private string _n;
        private string _p2O5;
        private string _k2O;

        private ICommand _saveCommand;

        private readonly Analise _analise;
        private readonly Realm _realm;

        #endregion

        #region Binding properties

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

        public Cultura Cultura
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
    }
}