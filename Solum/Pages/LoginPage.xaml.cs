using Solum.Effects;
using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Messages.LoginMessages;
using static Solum.Settings;

namespace Solum.Pages
{
    public partial class LoginPage : ContentPage
    {
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
            MessagingCenter.Subscribe<LoginViewModel>(this, EntryNullValuesTitle, async (sender) =>
            {
                await DisplayAlert(ErrorMessageTitle, EntryNullValuesMessage, ButtonTitle);
            }); 

            MessagingCenter.Subscribe<LoginViewModel>(this, LoginErrorTitle, async (sender) =>
            {
                await DisplayAlert(ErrorMessageTitle, LoginErrorMessage, ButtonTitle);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LoginViewModel, string>(this, EntryNullValuesTitle);
            MessagingCenter.Unsubscribe<LoginViewModel, string>(this, LoginErrorTitle);
        }
    }
}