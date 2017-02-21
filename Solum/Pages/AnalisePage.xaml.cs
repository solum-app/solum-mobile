using System;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class AnalisePage : ContentPage
    {
        public AnalisePage()
        {
            InitializeComponent();
            BindingContext = new AnaliseViewModel(Navigation);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }

        public AnalisePage(string analiseId)
        {
            InitializeComponent();
            BindingContext = new AnaliseViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }

        private void OnEntryFocused(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.Android)
                boxSpacing.IsVisible = !boxSpacing.IsVisible;
        }

        private void OnEntryUnfocused(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.Android)
                boxSpacing.IsVisible = false;
        }
    }
}