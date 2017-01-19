using Solum.Effects;
using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Messages.LoginMessages;

namespace Solum.Pages
{
    public partial class LoginPage : ContentPage
    {
        private static string _buttonTittle = "Ok";
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);
            NavigationPage.SetHasNavigationBar(this, false); 
			if (Device.OS == TargetPlatform.Android)
                DependencyService.Get<IStatusBarColor>().SetColor((Color)Application.Current.Resources["loginBackgroundDark"]);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<LoginViewModel, string>(this, EntryNullValuesTitle, async (sender, args) =>
            {
                await DisplayAlert("Ops! :-/", EntryNullValuesMessage, _buttonTittle);
            }); 

            MessagingCenter.Subscribe<LoginViewModel, string>(this, LoginErrorTitle, async (sender, args) =>
            {
                await DisplayAlert("Ops! :-/", LoginErrorMessage, _buttonTittle);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LoginViewModel, string>(this, "NullEntrys");
            MessagingCenter.Unsubscribe<LoginViewModel, string>(this, "LoginError");
        }
    }
}