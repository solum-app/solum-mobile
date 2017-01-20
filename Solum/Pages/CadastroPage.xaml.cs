using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Messages.RegisterMessages;
using static Solum.Settings;

namespace Solum.Pages
{
    public partial class CadastroPage : ContentPage
    {
        public CadastroPage()
        {
            InitializeComponent();
            BindingContext = new CadastroViewModel(Navigation);
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<CadastroViewModel>(this, EntryNullValuesTitle,
                async view => { await DisplayAlert(ErrorMessageTitle, EntryNullValuesMessage, ButtonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, InvalidEmailTitle,
                async view => { await DisplayAlert(ErrorMessageTitle, InvalidEmailMessage, ButtonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, PasswordIsntMatchTitle,
                async view => { await DisplayAlert(ErrorMessageTitle, PasswordIsntMatchMessage, ButtonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, WeakPasswordTitle,
                async view => { await DisplayAlert(ErrorMessageTitle, WeakPasswordMessage, ButtonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, CityIsntSelectedTitle,
                async view => { await DisplayAlert(ErrorMessageTitle, CityIsntSelectedMessage, ButtonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, RegisterUnsuccessTitle,
                async view => { await DisplayAlert(ErrorMessageTitle, RegisterUnsuccessMessage, ButtonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, RegisterSucessfullTitle,
                async view => { await DisplayAlert(SuccessMessageTitle, RegisterSuccessfullMessage, ButtonTitle); });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CadastroViewModel>(this, EntryNullValuesTitle);
            MessagingCenter.Unsubscribe<CadastroViewModel>(this, InvalidEmailTitle);
            MessagingCenter.Unsubscribe<CadastroViewModel>(this, PasswordIsntMatchTitle);
            MessagingCenter.Unsubscribe<CadastroViewModel>(this, WeakPasswordTitle);
            MessagingCenter.Unsubscribe<CadastroViewModel>(this, CityIsntSelectedTitle);
            MessagingCenter.Unsubscribe<CadastroViewModel>(this, RegisterSucessfullTitle);
        }
    }
}