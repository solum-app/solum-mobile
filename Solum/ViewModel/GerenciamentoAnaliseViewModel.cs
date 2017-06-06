using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class GerenciamentoAnaliseViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private Analise _analise;
        private string _calagemDate;
        private string _coberturaDate;
        private string _corretivaDate;
        private ICommand _generatePdfCommand;
        private bool _hasCalagemCalculation;
        private bool _hasCoberturaCalculation;
        private bool _hasCorretivaCalculation;
        private bool _hasSemeaduraCalculation;
        private string _interpretacaoDate;
        private string _semeaduraDate;
        private ICommand _showCoberturaPageCommand;
        private ICommand _showCorretivaPageCommand;
        private ICommand _showInterpretacaoPageCommand;
        private ICommand _showRecomendacaoCalagemPageCommand;
        private ICommand _showSemeaduraPageCommand;
        private bool _wasInterpreted;

		public GerenciamentoAnaliseViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
			AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(
				task =>
				  {
					  Analise = task.Result;
					  UpdateValues(Analise);
				  }
			);

            _userDialogs = DependencyService.Get<IUserDialogs>();
        }

        public Analise Analise
        {
			get { return _analise; }
			set { SetPropertyChanged(ref _analise, value); }
        }

        public bool WasInterpreted
        {
			get { return _wasInterpreted; }
			set { SetPropertyChanged(ref _wasInterpreted, value); }
        }

        public bool HasCalagemCalculation
        {
			get { return _hasCalagemCalculation; }
			set { SetPropertyChanged(ref _hasCalagemCalculation, value); }
        }

        public bool HasCorretivaCalculation
        {
			get { return _hasCorretivaCalculation; }
			set { SetPropertyChanged(ref _hasCorretivaCalculation, value); }
        }

        public bool HasSemeaduraCalculation
        {
			get { return _hasSemeaduraCalculation; }
			set { SetPropertyChanged(ref _hasSemeaduraCalculation, value); }
        }

        public bool HasCoberturaCalculation
        {
			get { return _hasCoberturaCalculation; }
			set { SetPropertyChanged(ref _hasCoberturaCalculation, value); }
        }

        public string InterpretacaoDate
        {
			get { return _interpretacaoDate; }
			set { SetPropertyChanged(ref _interpretacaoDate, value); }
        }

        public string CalagemDate
        {
			get { return _calagemDate; }
			set { SetPropertyChanged(ref _calagemDate, value); }
        }

        public string CorretivaDate
        {
			get { return _corretivaDate; }
			set { SetPropertyChanged(ref _corretivaDate, value); }
        }

        public string SemeaduraDate
        {
			get { return _semeaduraDate; }
			set { SetPropertyChanged(ref _semeaduraDate, value); }
        }

        public string CoberturaDate
        {
			get { return _coberturaDate; }
			set { SetPropertyChanged(ref _coberturaDate, value); }
        }


        public ICommand ShowInterpretacaoPageCommand
			=> _showInterpretacaoPageCommand ?? (_showInterpretacaoPageCommand = new Command(async ()=> await ShowInterpretacaoPage()));

        public ICommand ShowRecomendacaoCalagemPageCommand
			=> _showRecomendacaoCalagemPageCommand ?? (_showRecomendacaoCalagemPageCommand = new Command((async ()=> await ShowRecomendacaoCalagemPage())));

        public ICommand ShowCorretivaPageCommand
			=> _showCorretivaPageCommand ?? (_showCorretivaPageCommand = new Command((async ()=> await ShowCorretivaPage())));

        public ICommand ShowSemeaduraPageCommand
			=> _showSemeaduraPageCommand ?? (_showSemeaduraPageCommand = new Command((async ()=> await ShowSemeaduraPage())));

        public ICommand ShowCoberturaPageCommand
			=> _showCoberturaPageCommand ?? (_showCoberturaPageCommand = new Command((async ()=> await ShowCoberturaPage())));

         public ICommand GeneratePdfCommand 
			=> _generatePdfCommand ?? (_generatePdfCommand = new Command((async ()=> await GeneratePdf())));


		private async Task ShowInterpretacaoPage()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new InterpretacaoPage(Analise.Id));
                IsBusy = false;
            }
        }

		private async Task ShowCalagemPage()
        {
            if (!WasInterpreted)
            {
                await _userDialogs.DisplayAlert(MessagesResource.InterpretacaoNull);
                return;
            }
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new CalagemPage(Analise.Id));
                IsBusy = false;
            }
        }

		private async Task ShowRecomendacaoCalagemPage()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                if (HasCalagemCalculation)
                {
                    await Navigation.PushAsync(new RecomendacaoCalagemPage(Analise.Id, Analise.V2, Analise.Prnt, Analise.Profundidade, true));
                    IsBusy = false;
                }
                else
                {
                    IsBusy = false;
                    await ShowCalagemPage();
                }
            }
        }

		private async Task ShowCorretivaPage()
        {
            if (!HasCalagemCalculation)
            {
				await _userDialogs.DisplayAlert(MessagesResource.CalagemNull);
                return;
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new AdubacaoCorretivaPage(Analise.Id, !Analise.HasCorretiva));
                IsBusy = false;
            }
        }

		private async Task ShowSemeaduraPage()
        {
            if (!HasCorretivaCalculation)
            {
                await _userDialogs.DisplayAlert(MessagesResource.CorretivaNull);
                return;
            }
            if (IsNotBusy)
            {
                IsBusy = true;
                if (HasSemeaduraCalculation)
                {
                    Cultura c;
                    Enum.TryParse(Analise.Cultura, out c);
                    await Navigation.PushAsync(
                        new RecomendacaoSemeaduraPage(Analise.Id, Analise.Expectativa, c, Analise.HasSemeadura));
                }
                else
                {
                    await Navigation.PushAsync(new SemeaduraPage(Analise.Id));
                }
                IsBusy = false;
            }
        }

		private async Task ShowCoberturaPage()
        {
            if (!HasSemeaduraCalculation)
            {
                await _userDialogs.DisplayAlert(MessagesResource.SemeaduraNull);
                return;
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new AdubacaoCoberturaPage(Analise.Id, !Analise.HasCobertura));
                IsBusy = false;
            }
        }

		public void RefreshValues()
		{
			if (Analise != null)
			{
				var analiseId = Analise.Id;
				AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(
					task =>
					  {
						  Analise = task.Result;
						  UpdateValues(Analise);
					  }
				);
			}
		}

		public void UpdateValues(Analise analise)
		{
			WasInterpreted = analise.WasInterpreted;
			HasCalagemCalculation = analise.HasCalagem;
			HasCorretivaCalculation = analise.HasCorretiva;
			HasSemeaduraCalculation = analise.HasSemeadura;
			HasCoberturaCalculation = analise.HasCobertura;
			InterpretacaoDate = analise.WasInterpreted ? $"Realizada em  {analise.DataInterpretacao:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
			CalagemDate = analise.HasCalagem ? $"Realizada em {analise.DataCalculoCalagem:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
			CorretivaDate = analise.HasCorretiva ? $"Realizada em {analise.DataCalculoCorretiva:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
			SemeaduraDate = analise.HasSemeadura ? $"Realizada em {analise.DataCalculoSemeadura:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
			CoberturaDate = analise.HasCobertura ? $"Realizada em {analise.DataCalculoCobertura:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
		}

		private async Task GeneratePdf()
        {
			if (!WasInterpreted)
			{
                await _userDialogs.DisplayAlert(MessagesResource.InterpretacaoNull);
				return;
			}

            IsBusy = true;
            var stream = await new ReportsGenerator().GeneratePDFReport(_analise);
            DependencyService.Get<IPdfViewer>().PreviewPdf(stream);
            IsBusy = false;
        }


    }
}