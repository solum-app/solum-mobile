using System;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
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
            PageTitle = Analise.Nome;
           UpdateValues();
        }

        #region Private Properties

        private ICommand _showCalagemPageCommand;
        private ICommand _showCoberturaPageCommand;
        private ICommand _showCorretivaPageCommand;
        private ICommand _showInterpretacaoPageCommand;
        private ICommand _showSemeaduraPageCommand;

        private bool _hasInterpretacaoAccomplished;
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
        private Realm _realm;

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

        public bool HasInterpretacaoAccomplished
        {
            get { return _hasInterpretacaoAccomplished; }
            set { SetPropertyChanged(ref _hasInterpretacaoAccomplished, value); }
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

        public DateTimeOffset? InterpretacaoDate
        {
            get { return _interpretacaoDate; }
            set
            {
                SetPropertyChanged(ref _interpretacaoDate, value);
                HasInterpretacaoAccomplished = InterpretacaoDate.HasValue;
            }
        }

        public DateTimeOffset? CalagemDate
        {
            get { return _calagemDate; }
            set
            {
                SetPropertyChanged(ref _calagemDate, value);
                HasCalagemCalculation = CalagemDate.HasValue;
            }
        }

        public DateTimeOffset? CorretivaDate
        {
            get { return _corretivaDate; }
            set
            {
                SetPropertyChanged(ref _corretivaDate, value);
                HasCorretivaCalculation = CorretivaDate.HasValue;
            }
        }

        public DateTimeOffset? SemeaduraDate
        {
            get { return _semeaduraDate; }
            set
            {
                SetPropertyChanged(ref _semeaduraDate, value);
                HasSemeaduraCalculation = SemeaduraDate.HasValue;
            }
        }

        public DateTimeOffset? CoberturaDate
        {
            get { return _coberturaDate; }
            set
            {
                SetPropertyChanged(ref _coberturaDate, value);
                HasCoberturaCalculation = CoberturaDate.HasValue;
            }
        }

        #endregion

        #region Commands

        public ICommand ShowInterpretacaoPageCommand
            => _showInterpretacaoPageCommand ?? (_showInterpretacaoPageCommand = new Command(ShowInterpretacaoPage));

        public ICommand ShowCalagemPageCommand
            => _showCalagemPageCommand ?? (_showCalagemPageCommand = new Command(ShowCalagemPage));

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
                await Navigation.PushAsync(new InterpretacaoPage(Navigation, Analise));
                IsBusy = false;
            }
        }

        private async void ShowCalagemPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if (!HasInterpretacaoAccomplished)
                    await Navigation.PushAsync(new CalagemPage(Analise));
                IsBusy = false;
            } 
        }

        private async void ShowCorretivaPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if(HasCalagemCalculation)
                    await Navigation.PushAsync(new CorretivaPage(Analise));
                IsBusy = false;
            }
        }

        private async void ShowSemeaduraPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if (HasCorretivaCalculation)
                    await Navigation.PushAsync(new SemeaduraPage(Analise));
                IsBusy = false;
            }
        }

        private async void ShowCoberturaPage()
        {
            if (!IsBusy && HasSemeaduraCalculation)
            {
                IsBusy = true;
                await Navigation.PushAsync(new CoberturaPage(Analise));
                IsBusy = false;
            }
        }

        public void UpdateValues()
        {
            Analise = _realm.Find<Analise>(Analise.Id);
            InterpretacaoDate = Analise.DataInterpretacao;
            CalagemDate = Analise.DataCalculoCalagem;
            CorretivaDate = Analise.DataCalculoCorretiva;
            SemeaduraDate = Analise.DataCalculoSemeadura;
            CoberturaDate = Analise.DataCalculoCobertura;
        }
        #endregion
    }
}