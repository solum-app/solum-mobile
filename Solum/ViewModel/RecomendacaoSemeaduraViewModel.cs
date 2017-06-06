using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class RecomendacaoSemeaduraViewModel : BaseViewModel
    {
        private Analise _analise;
        private Cultura _cultura;
        private bool _enableButton;
		private bool _allowEdit;
        private int _expectativa;
        private float _k2O;
        private float _n;
        private float _p2O5;
        private ICommand _showSemeaduraPageCommand;

        public RecomendacaoSemeaduraViewModel(INavigation navigation, string analiseId, int expectativa, Cultura cultura, bool allowEdit) : base(navigation)
        {
            AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(t =>
            {
                _analise = t.Result;
                PageTitle = _analise.Identificacao;
                Init(expectativa, cultura);
            });
            
            EnableButton = !allowEdit;
			_allowEdit = allowEdit;
        }


        public ICommand SalvarCommand
			=> _showSemeaduraPageCommand ?? (_showSemeaduraPageCommand = new Command(async ()=> await Salvar()));


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
            var interpreter = SemeaduraInjector.GetInstance(cultura);
            Expectativa = expectativa;
            Cultura = cultura;
            N = interpreter.QuanidadeNitrogenio(expectativa, Nivel.Adequado);
            P2O5 = interpreter.QuantidadeFosforo(expectativa, Interpretador.NiveFosforo(_analise.Fosforo, Interpretador.Textura(_analise.Argila, _analise.Silte)));
            K2O = interpreter.QuantidadePotassio(expectativa, Interpretador.NivelPotassio(_analise.Potassio, _analise.CTC));
        }


        private async Task Salvar()
        {
            if (!IsNotBusy) return;
            IsBusy = true;
            _analise.DataCalculoSemeadura = DateTimeOffset.Now;
            _analise.HasSemeadura = true;
            _analise.Cultura = Cultura.ToString();
            _analise.Expectativa = Expectativa;
			await AzureService.Instance.AddOrUpdateAnaliseAsync(_analise);
			var previous = Navigation.NavigationStack[Navigation.NavigationStack.Count -2];
			var beforePrevious = Navigation.NavigationStack[Navigation.NavigationStack.Count -3];
			Navigation.RemovePage(previous);
			if (beforePrevious != null && beforePrevious.GetType() == typeof(RecomendacaoSemeaduraPage))
				Navigation.RemovePage(beforePrevious);
            await Navigation.PopAsync();
            IsBusy = false;
        }
    }
}