using System.Windows.Input;
using Realms;
using Solum.Models;
using Solum.Pages;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class GerenciamentoAnaliseViewModel : BaseViewModel
    {
        private ICommand _abrirTelaCalagemCommand;
        private ICommand _abrirTelaCoberturaCommand;
        private ICommand _abrirTelaCorretivaCommand;
        private ICommand _abrirTelaInterpretacaoCommand;
        private ICommand _abrirTelaSemeaduraCommand;

        private Analise _analise;

        private Realm _realm;

        public GerenciamentoAnaliseViewModel(INavigation navigation, Analise analise) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Analise = analise;
        }

        public Analise Analise
        {
            get { return _analise; }
            set { SetPropertyChanged(ref _analise, value); }
        }

        public ICommand AbriTelaInterpretacaoCommand
            => _abrirTelaInterpretacaoCommand ?? (_abrirTelaInterpretacaoCommand = new Command(AbrirTelaInterpretacao));

        public ICommand AbrirTelaCalagemCommand
            => _abrirTelaCalagemCommand ?? (_abrirTelaCalagemCommand = new Command(AbrirTelaCalagem));

        public ICommand AbrirTelaCorretivaCommand
            => _abrirTelaCorretivaCommand ?? (_abrirTelaCorretivaCommand = new Command(AbrirTelaCorretiva));

        public ICommand AbrirTelaSemeaduraCommand
            => _abrirTelaSemeaduraCommand ?? (_abrirTelaSemeaduraCommand = new Command(AbrirTelaSemeadura));

        public ICommand AbrirTelaCoberturaCommand
            => _abrirTelaCoberturaCommand ?? (_abrirTelaCoberturaCommand = new Command(AbrirTelaCobertura));

        private async void AbrirTelaInterpretacao()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new InterpretacaoPage(Analise, true));
                IsBusy = false;
            }
        }

        private async void AbrirTelaCalagem()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new Pages.CalagemPage());
                IsBusy = false;
            }
        }

        private async void AbrirTelaCorretiva()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new Pages.CorretivaPage());
                IsBusy = false;
            }
        }

        private async void AbrirTelaSemeadura()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new Pages.SemeaduraPage());
                IsBusy = false;
            }
        }

        private async void AbrirTelaCobertura()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new CoberturaPage());
                IsBusy = false;
            }
        }
    }
}