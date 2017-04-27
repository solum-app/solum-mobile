using System;
using System.Linq;
using System.Windows.Input;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class RecomendacaoSemeaduraViewModel : BaseViewModel
    {
        private Analise _analise;
        private readonly ISemeaduraInterpreter _interpreter;
        private Cultura _cultura;


        private bool _enableButton;

        private string _expectativa;
        private string _k20;
        private string _n;
        private string _p205;

        private ICommand _showSemeaduraPageCommand;

        public RecomendacaoSemeaduraViewModel(INavigation navigation, string analiseId, int expectativa,
            Cultura cultura, bool enableButton) : base(navigation)
        {
            _interpreter = SemeaduraInjector.GetInstance(cultura);
            AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(t =>
            {
                _analise = t.Result;
                PageTitle = _analise.Identificacao;
                Init(expectativa, cultura);
            });
            
            EnableButton = enableButton;
        }


        public ICommand SalvarCommand
            => _showSemeaduraPageCommand ?? (_showSemeaduraPageCommand = new Command(Salvar));


        public bool EnableButton
        {
            get => _enableButton;
            set => SetPropertyChanged(ref _enableButton, value);
        }

        public string Expectativa
        {
            get => _expectativa;
            set => SetPropertyChanged(ref _expectativa, value);
        }

        public Cultura Cultura
        {
            get => _cultura;
            set => SetPropertyChanged(ref _cultura, value);
        }

        public string N
        {
            get => _n;
            set => SetPropertyChanged(ref _n, value);
        }

        public string P205
        {
            get => _p205;
            set => SetPropertyChanged(ref _p205, value);
        }

        public string K20
        {
            get => _k20;
            set => SetPropertyChanged(ref _k20, value);
        }


        private void Init(int expectativa, Cultura cultura)
        {
            Expectativa = expectativa.ToString();
            Cultura = cultura;
            N = _interpreter.QuanidadeNitrogenio(expectativa, Nivel.Adequado).ToString();
            P205 =
                _interpreter.QuantidadeFosforo(expectativa,
                        Interpretador.NiveFosforo(_analise.Fosforo,
                            Interpretador.Textura(_analise.Argila, _analise.Silte)))
                    .ToString();
            K20 =
                _interpreter.QuantidadePotassio(expectativa,
                        Interpretador.NivelPotassio(_analise.Potassio, _analise.CTC))
                    .ToString();
        }


        private async void Salvar()
        {
            if (!IsNotBusy) return;
            IsBusy = true;
            //var count = Navigation.NavigationStack.Count();
            
            _analise.DataCalculoSemeadura = DateTimeOffset.Now;
            _analise.HasSemeadura = true;
            _analise.Cultura = Cultura.ToString();
            _analise.Expectativa = int.Parse(Expectativa);
            await AzureService.Instance.UpdateAnaliseAsync(_analise);
            var last = Navigation.NavigationStack.LastOrDefault();
            Navigation.RemovePage(last);
            await Navigation.PopAsync();
            IsBusy = false;
        }
    }
}