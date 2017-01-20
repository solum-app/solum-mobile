using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Messages.FazendaMessages;

namespace Solum.Pages
{
    public partial class FazendaCadastroPage : ContentPage
    {
        private static string _buttonTitle = "OK";
        public FazendaCadastroPage()
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }

        public FazendaCadastroPage(Fazenda fazenda)
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation, fazenda);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<FazendaCadastroViewModel>(this, NullEntriesTitle, async view =>
            {
                await DisplayAlert("Ops! :-/", NullEntriesMessage, _buttonTitle);
            });

            MessagingCenter.Subscribe<FazendaCadastroViewModel>(this, RegisterSuccessfullTitle, async view =>
            {
                await DisplayAlert("Tudo Certo! ;-)", RegisterSuccessfullMessage, _buttonTitle);
            });

            MessagingCenter.Subscribe<FazendaCadastroViewModel>(this, UpdateSuccessfullTitle, async view =>
            {
                await DisplayAlert("Tudo Certo! ;-)", UpdateSucessfullMessage, _buttonTitle);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<FazendaCadastroViewModel>(this, NullEntriesTitle);
            MessagingCenter.Unsubscribe<FazendaCadastroViewModel>(this, RegisterSuccessfullTitle);
            MessagingCenter.Unsubscribe<FazendaCadastroViewModel>(this, UpdateSuccessfullTitle);
        }
    }
}