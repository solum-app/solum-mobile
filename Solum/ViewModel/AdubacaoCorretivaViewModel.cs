using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Handlers;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class AdubacaoCorretivaViewModel : BaseViewModel
    {
        private Analise _analise;
        private bool _enableButton;
        private string _k2O;
        private string _p2O5;
        private ICommand _salvarCommand;

        public AdubacaoCorretivaViewModel(INavigation navigation, string analiseId,
            bool enableButton) : base(navigation)
        {
            AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(t =>
            {
                _analise = t.Result;
                PageTitle = _analise.Identificacao;
                EnableButton = enableButton;
                Calculate();
            });
        }


		public ICommand SalvarCommand => _salvarCommand ?? (_salvarCommand = new Command(async ()=> await Salvar()));

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

        public bool EnableButton
        {
			get { return _enableButton; }
			set { SetPropertyChanged(ref _enableButton, value); }
        }


        private void Calculate()
        {
            var argila = _analise.Argila;

            var textura = Interpretador.Textura(_analise.Argila, _analise.Silte);
            var pInterpretaded = Interpretador.NiveFosforo(_analise.Fosforo, textura);
            var kInterpretaded = Interpretador.NivelPotassio(_analise.Potassio, _analise.CTC);

            if (pInterpretaded != Nivel.Adequado && pInterpretaded != Nivel.Alto)
                switch (pInterpretaded)
                {
                    case Nivel.MuitoBaixo:
                        P2O5 = (argila / 10 * 4).ToString("###");
                        break;
                    case Nivel.Baixo:
                        P2O5 = (argila / 10 * 2).ToString("###");
                        break;
                    case Nivel.Medio:
                        P2O5 = (argila / 10 * 1).ToString("###");
                        break;
                }
            else
                P2O5 = 0.ToString();

            if (kInterpretaded != Nivel.Adequado && kInterpretaded != Nivel.Alto)
            {
                var ctc = _analise.CTC;
                if (ctc < 4)
                    switch (kInterpretaded)
                    {
                        case Nivel.Baixo:
                            K2O = 50.0f.ToString("###");
                            break;
                        case Nivel.Medio:
                            K2O = 25.0f.ToString("###");
                            break;
                    }
                else
                    switch (kInterpretaded)
                    {
                        case Nivel.Baixo:
                            K2O = 100.0f.ToString("###");
                            break;
                        case Nivel.Medio:
                            K2O = 50.0f.ToString("###");
                            break;
                    }
            }
            else
            {
                K2O = 0.ToString();
            }
        }


		private async Task Salvar()
        {
			if (IsBusy) return;
            IsBusy = true;
            _analise.DataCalculoCorretiva = DateTimeOffset.Now;
            _analise.HasCorretiva = true;
            await AzureService.Instance.UpdateAnaliseAsync(_analise);
            MessagesResource.DadosSalvos.ToToast();
            await Navigation.PopAsync();
            IsBusy = false;
        }
    }
}