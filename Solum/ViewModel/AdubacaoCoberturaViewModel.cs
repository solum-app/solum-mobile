using System;
using System.Threading.Tasks;
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
        private readonly IUserDialogs _userDialogs;
        private Analise _analise;
        private Cultura _cultura;
        private bool _enableButton;
        private int _expectativa;
        private float _k2O;
        private float _n;
        private float _p2O5;
        private ICommand _saveCommand;

        public AdubacaoCoberturaViewModel(INavigation navigation, string analiseid,
            bool enableButton) : base(navigation)
        {
            AzureService.Instance.FindAnaliseAsync(analiseid)
                .ContinueWith(t =>
                {
                    _analise = t.Result;
                    PageTitle = _analise.Identificacao;
                    Enum.TryParse(_analise.Cultura, out Cultura c);
                    Cultura = c;
                    Expectativa = _analise.Expectativa;
                    Init(Expectativa, Cultura);
                    EnableButton = enableButton;
                });

            _userDialogs = DependencyService.Get<IUserDialogs>();
        }

		public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new Command(async ()=> await Salvar()));

        public bool EnableButton
        {
			get { return _enableButton; }
			set { SetPropertyChanged(ref _enableButton, value); }
        }

        public int Expectativa
        {
			get { return _expectativa; }
			set { SetPropertyChanged(ref _expectativa, value); }
        }

        public Cultura Cultura
        {
			get { return _cultura; }
			set { SetPropertyChanged(ref _cultura, value); }
        }

        public float N
        {
			get { return _n; }
			set { SetPropertyChanged(ref _n, value); }
        }

        public float P2O5
        {
			get { return _p2O5; }
			set { SetPropertyChanged(ref _p2O5, value); }
        }

        public float K2O
        {
			get { return _k2O; }
			set { SetPropertyChanged(ref _k2O, value); }
        }


        private void Init(int expectativa, Cultura cultura)
        {
            var calculator = CoberturaInjector.GetInstance(cultura);
            N = calculator.QuanidadeNitrogenio(expectativa);
            P2O5 = calculator.QuantidadeFosforo(expectativa);
            K2O = calculator.QuantidadePotassio(expectativa);
        }

		private async Task Salvar()
        {
            IsBusy = true;
            _analise.DataCalculoCobertura = DateTimeOffset.Now;
            _analise.HasCobertura = true;
			await AzureService.Instance.AddOrUpdateAnaliseAsync(_analise);
            _userDialogs.ShowToast(MessagesResource.DadosSalvos);
            await Navigation.PopAsync();
            IsBusy = false;
        }
    }
}