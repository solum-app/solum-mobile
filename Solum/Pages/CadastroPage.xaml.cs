using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Messages.RegisterMessages;

namespace Solum.Pages
{
    public partial class CadastroPage : ContentPage
    {
        private static readonly string _buttonTitle = "OK";

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
                async view => { await DisplayAlert("Ops! :-/", EntryNullValuesMessage, _buttonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, InvalidEmailTitle,
                async view => { await DisplayAlert("Ops! :-/", InvalidEmailMessage, _buttonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, PasswordIsntMatchTitle,
                async view => { await DisplayAlert("Ops! :-/", PasswordIsntMatchMessage, _buttonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, WeakPasswordTitle,
                async view => { await DisplayAlert("Ops! :-/", WeakPasswordMessage, _buttonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, CityIsntSelectedTitle,
                async view => { await DisplayAlert("Ops! :-/", CityIsntSelectedMessage, _buttonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel>(this, RegisterUnsuccessTitle,
                async view => { await DisplayAlert("Ops! :-/", RegisterUnsuccessMessage, _buttonTitle); });
            MessagingCenter.Subscribe<CadastroViewModel>(this, RegisterSucessfullTitle,
                async view => { await DisplayAlert("Tudo certo! ;-)", RegisterSuccessfullMessage, _buttonTitle); });
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