using System;
using System.Collections.Generic;
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
    public class RecomendacaoCalagemViewModel : BaseViewModel
    {
        private Analise _analise;
        private IUserDialogs _userDialogs;

        private readonly IDictionary<int, float> _values = new Dictionary<int, float>
        {
            {5, 0.25f},
            {10, 0.5f},
            {20, 1.0f},
            {40, 2.0f}
        };

        private bool _enableButton;
        private float _prnt;
        private int _profundidade;
        private float _quantidade;
        private ICommand _salvarCommand;
        private float _v2;

        public RecomendacaoCalagemViewModel(INavigation navigation, string analiseId, float v2, float prnt,
            int profundidade, bool allowEdit) : base(navigation)
        {
            AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(t =>
            {
                _analise = t.Result;
                PageTitle = _analise.Identificacao;
                QuantidadeCal = Calculador.CalcularCalcario(_analise.Prnt, _analise.V2, _analise.CTC, _analise.V, _analise.Profundidade);
            });

            V2 = v2;
            Prnt = prnt;
            Profundidade = profundidade;

			EnableButton = !allowEdit;
            _userDialogs = DependencyService.Get<IUserDialogs>();
        }

        public ICommand SalvarCommand => _salvarCommand ?? (_salvarCommand = new Command(async () => await Salvar()));


        public bool EnableButton
        {
			get { return _enableButton; }
			set { SetPropertyChanged(ref _enableButton, value); }
        }

        public float QuantidadeCal
        {
			get { return _quantidade; }
			set { SetPropertyChanged(ref _quantidade, value); }
        }

        public float V2
        {
			get { return _v2; }
			set { SetPropertyChanged(ref _v2, value); }
        }

        public float Prnt
        {
			get { return _prnt; }
			set { SetPropertyChanged(ref _prnt, value); }
        }

        public int Profundidade
        {
			get { return _profundidade; }
			set { SetPropertyChanged(ref _profundidade, value); }
        }


        private async Task Salvar()
        {
            IsBusy = true;
            _analise.V2 = V2;
            _analise.Prnt = Prnt;
            _analise.Profundidade = Profundidade;
            _analise.DataCalculoCalagem = DateTimeOffset.Now;
            _analise.HasCalagem = true;
			await AzureService.Instance.AddOrUpdateAnaliseAsync(_analise);
            _userDialogs.ShowToast(MessagesResource.DadosSalvos);

			var previous = Navigation.NavigationStack[Navigation.NavigationStack.Count - 2];
			var beforePrevious = Navigation.NavigationStack[Navigation.NavigationStack.Count - 3];
			Navigation.RemovePage(previous);
			if (beforePrevious != null && beforePrevious.GetType() == typeof(RecomendacaoCalagemPage))
				Navigation.RemovePage(beforePrevious);
			
            await Navigation.PopAsync();
            IsBusy = false;
        }
    }
}