using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Messages.TalhaoMessages;
using static Solum.Settings;

namespace Solum.Pages
{
    public partial class TalhaoCadastroPage : ContentPage
    {
        public TalhaoCadastroPage(Fazenda fazenda, bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, fazenda, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }

        public TalhaoCadastroPage(Talhao talhao)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, talhao);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<TalhaoCadastroViewModel>(this, NullEntriesTitle,
                async model => { await DisplayAlert(ErrorMessageTitle, NullEntriesMessage, ButtonTitle); });

            MessagingCenter.Subscribe<TalhaoCadastroViewModel>(this, RegisterSuccessfullTitle,
                async model => { await DisplayAlert(SuccessMessageTitle, RegisterSuccessfullMessage, ButtonTitle); });

            MessagingCenter.Subscribe<TalhaoCadastroViewModel>(this, UpdateSuccessfullTitle,
                async model => { await DisplayAlert(SuccessMessageTitle, UpdateSucessfullMessage, ButtonTitle); });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel>(this, NullEntriesTitle);
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel>(this, RegisterSuccessfullTitle);
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel>(this, UpdateSuccessfullTitle);
        }
    }
}