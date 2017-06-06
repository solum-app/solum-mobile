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
    public class AdubacaoCorretivaViewModel : BaseViewModel
    {
        private Analise _analise;
        private bool _enableButton;
        private float _k2O;
        private float _p2O5;
        private ICommand _salvarCommand;
        private readonly IUserDialogs _userDialogs;

        public AdubacaoCorretivaViewModel(INavigation navigation, string analiseId,
            bool enableButton) : base(navigation)
        {
            AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(t =>
            {
                _analise = t.Result;
                PageTitle = _analise.Identificacao;
                EnableButton = enableButton;
                K2O = Calculador.CalcularK2O(_analise.Argila, _analise.Silte, _analise.Potassio, _analise.CTC);
                P2O5 = Calculador.CalcularP2O5(_analise.Argila, _analise.Silte, _analise.Fosforo);
            });

            _userDialogs = DependencyService.Get<UserDialogs>();
        }


		public ICommand SalvarCommand => _salvarCommand ?? (_salvarCommand = new Command(async ()=> await Salvar()));

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

        public bool EnableButton
        {
			get { return _enableButton; }
			set { SetPropertyChanged(ref _enableButton, value); }
        }

		private async Task Salvar()
        {
            IsBusy = true;
            _analise.DataCalculoCorretiva = DateTimeOffset.Now;
            _analise.HasCorretiva = true;
			await AzureService.Instance.AddOrUpdateAnaliseAsync(_analise);
            _userDialogs.ShowToast(MessagesResource.DadosSalvos);
            await Navigation.PopAsync();
            IsBusy = false;
        }
    }
}