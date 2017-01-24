using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Messages.FazendaMessages;
using static Solum.Settings;

namespace Solum.Pages
{
    public partial class FazendaCadastroPage : ContentPage
    {
        public FazendaCadastroPage(bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }

        public FazendaCadastroPage(Fazenda fazenda)
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation, fazenda);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<FazendaCadastroViewModel>(this, NullEntriesTitle, async view =>
            {
                await DisplayAlert(ErrorMessageTitle, NullEntriesMessage, ButtonTitle);
            });

            MessagingCenter.Subscribe<FazendaCadastroViewModel>(this, RegisterSuccessfullTitle, async view =>
            {
                await DisplayAlert(SuccessMessageTitle, RegisterSuccessfullMessage, ButtonTitle);
            });

            MessagingCenter.Subscribe<FazendaCadastroViewModel>(this, UpdateSuccessfullTitle, async view =>
            {
                await DisplayAlert(SuccessMessageTitle, UpdateSucessfullMessage, ButtonTitle);
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