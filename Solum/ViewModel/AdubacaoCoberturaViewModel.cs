using System;
using System.Windows.Input;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AdubacaoCoberturaViewModel : BaseViewModel
    {
        private Analise _analise;
        private Cultura _cultura;


        private bool _enableButton;
        private string _expectativa;
        private string _k2O;

        private string _n;
        private string _p2O5;

        private ICommand _saveCommand;

        public AdubacaoCoberturaViewModel(INavigation navigation, string analiseid,
            bool enableButton) : base(navigation)
        {
            AzureService.Instance.FindAnaliseAsync(analiseid)
                .ContinueWith(t =>
                {
                    _analise = t.Result;
                    PageTitle = _analise.Identificacao;
                    Cultura c;
                    Enum.TryParse(_analise.Cultura, out c);
                    Cultura = c;
                    Expectativa = _analise.Expectativa.ToString();
                    Calculate();
                    EnableButton = enableButton;
                });
        }


        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(Salvar));


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


        private void Calculate()
        {
            var calculator = CoberturaInjector.GetInstance(Cultura);
            N = calculator?.CalculateN(_analise.Expectativa);
            P2O5 = calculator?.CalculateP(_analise.Expectativa);
            K2O = calculator?.CalculateK(_analise.Expectativa);
        }

        private async void Salvar()
        {
            if (!IsNotBusy) return;
            IsBusy = true;
            _analise.DataCalculoCobertura = DateTimeOffset.Now;
            _analise.HasCobertura = true;
            await AzureService.Instance.UpdateAnaliseAsync(_analise);
            MessagesResource.DadosSalvos.ToToast();
            await Navigation.PopAsync();
            IsBusy = false;
        }
    }
}