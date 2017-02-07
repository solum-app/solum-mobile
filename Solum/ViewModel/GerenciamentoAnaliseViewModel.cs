using System;
using System.Linq;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class GerenciamentoAnaliseViewModel : BaseViewModel
    {
        public GerenciamentoAnaliseViewModel(INavigation navigation, Analise analise) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Analise = _realm.Find<Analise>(analise.Id);
            PageTitle = $"Análise {Analise.Nome}";
            UpdateValues();
        }

        #region Private Properties

        private ICommand _showRecomendacaoCalagemPageCommand;
        private ICommand _showCoberturaPageCommand;
        private ICommand _showCorretivaPageCommand;
        private ICommand _showInterpretacaoPageCommand;
        private ICommand _showSemeaduraPageCommand;

        private bool _wasInterpreted;
        private bool _hasCalagemCalculation;
        private bool _hasCorretivaCalculation;
        private bool _hasSemeaduraCalculation;
        private bool _hasCoberturaCalculation;

        private string _pageTitle;

        private DateTimeOffset? _interpretacaoDate;
        private DateTimeOffset? _calagemDate;
        private DateTimeOffset? _corretivaDate;
        private DateTimeOffset? _semeaduraDate;
        private DateTimeOffset? _coberturaDate;

        private Analise _analise;
        private readonly Realm _realm;

        #endregion

        #region Binding Properties

        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetPropertyChanged(ref _pageTitle, value); }
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
            get
            {
                return _interpretacaoDate.HasValue
                    ? $"Realizada em  {_interpretacaoDate.Value:dd/MM/yy}"
                    : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _interpretacaoDate, DateTimeOffset.Parse(value)); }
        }

        public string CalagemDate
        {
            get
            {
                return _calagemDate.HasValue ? $"Realizada em {_calagemDate.Value:dd/MM/yy}" : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _calagemDate, DateTimeOffset.Parse(value)); }
        }

        public string CorretivaDate
        {
            get
            {
                return _corretivaDate.HasValue ? $"Realizada em {_corretivaDate.Value:dd/MM/yy}" : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _corretivaDate, DateTimeOffset.Parse(value)); }
        }

        public string SemeaduraDate
        {
            get
            {
                return _semeaduraDate.HasValue ? $"Realizada em {_semeaduraDate.Value:dd/MM/yy}" : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _semeaduraDate, DateTimeOffset.Parse(value)); }
        }

        public string CoberturaDate
        {
            get
            {
                return _coberturaDate.HasValue ? $"Realizada em {_coberturaDate.Value:dd/MM/yy}" : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _coberturaDate, DateTimeOffset.Parse(value)); }
        }

        #endregion

        #region Commands

        public ICommand ShowInterpretacaoPageCommand
            => _showInterpretacaoPageCommand ?? (_showInterpretacaoPageCommand = new Command(ShowInterpretacaoPage));

        public ICommand ShowRecomendacaoCalagemPageCommand
            => _showRecomendacaoCalagemPageCommand ??
               (_showRecomendacaoCalagemPageCommand = new Command(ShowRecomendacaoCalagemPage));

        public ICommand ShowCorretivaPageCommand
            => _showCorretivaPageCommand ?? (_showCorretivaPageCommand = new Command(ShowCorretivaPage));

        public ICommand ShowSemeaduraPageCommand
            => _showSemeaduraPageCommand ?? (_showSemeaduraPageCommand = new Command(ShowSemeaduraPage));

        public ICommand ShowCoberturaPageCommand
            => _showCoberturaPageCommand ?? (_showCoberturaPageCommand = new Command(ShowCoberturaPage));

        #endregion

        #region Functions

        private async void ShowInterpretacaoPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new InterpretacaoPage(Analise));
                IsBusy = false;
            }
        }

        private async void ShowCalagemPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if (WasInterpreted)
                    await Navigation.PushAsync(new CalagemPage(Analise.Id));
                else
                    "Você precisa realizar a interpretação primeiro".ToDisplayAlert(MessageType.Aviso);
                IsBusy = false;
            }
        }

        private async void ShowRecomendacaoCalagemPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
				var wasCalculated = _realm.All<Calagem>().Any(c => c.AnaliseId.Equals(Analise.Id));
				if (wasCalculated){
					var s = _realm.All<Calagem>().FirstOrDefault(c => c.AnaliseId.Equals(Analise.Id)).Id;
					await Navigation.PushAsync(new RecomendaCalagemPage(s));
                    IsBusy = false;
                }
				else
                {
					IsBusy = false;
					ShowCalagemPage();
				}
                
            }
        }

        private async void ShowCorretivaPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if(HasCalagemCalculation)
                    await Navigation.PushAsync(new CorretivaPage(Analise.Id));
                else
                    "Você precisa executar o cálculo para recomendação de calagem primeiro".ToDisplayAlert(MessageType.Aviso);
                IsBusy = false;
            }
        }

        private async void ShowSemeaduraPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if(HasCorretivaCalculation)
                    await Navigation.PushAsync(new SemeaduraPage(Analise.Id));
                else
                    "Você precisa executar o cálculo da recomendação corretiva primeiro".ToDisplayAlert(MessageType.Aviso);
                IsBusy = false;
            }
        }

        private async void ShowCoberturaPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if(HasSemeaduraCalculation)
                    await Navigation.PushAsync(new CoberturaPage(Analise.Id));
                else
                    "Você precisa executar a recomendação de semeadura primeiro".ToDisplayAlert(MessageType.Aviso);
                IsBusy = false;
            }
        }

        public void UpdateValues()
        {
            Analise = _realm.Find<Analise>(Analise.Id);
            if (Analise.DataInterpretacao != null)
            {
                InterpretacaoDate = Analise.DataInterpretacao.ToString();
                WasInterpreted = true;
            }
            if (Analise.DataCalculoCalagem != null)
            {
                CalagemDate = Analise.DataCalculoCalagem.ToString();
                HasCalagemCalculation = true;
            }
            if (Analise.DataCalculoCorretiva != null)
            {
                CorretivaDate = Analise.DataCalculoCorretiva.ToString();
                HasCorretivaCalculation = true;
            }
            if (Analise.DataCalculoSemeadura != null)
            {
                SemeaduraDate = Analise.DataCalculoSemeadura.ToString();
                HasSemeaduraCalculation = true;
            }
            if (Analise.DataCalculoCobertura != null)
            {
                CoberturaDate = Analise.DataCalculoCobertura.ToString();
                HasCoberturaCalculation = true;
            }
        }

        #endregion
    }
}