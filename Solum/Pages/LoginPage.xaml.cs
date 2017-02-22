using System;
using Solum.Effects;
using Solum.ViewModel;
using Xamarin.Forms;

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
                DependencyService.Get<IStatusBarColor>()
                    .SetColor((Color) Application.Current.Resources["loginBackgroundDark"]);
        }

        private void UsernameEntryOnCompleted(object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }

        private void PasswordEntryOnCompleted(object sender, EventArgs eventArgs)
        {
            PasswordEntry.Unfocus();
            (BindingContext as LoginViewModel)?.LoginCommand.Execute(null);
        }
    }
}