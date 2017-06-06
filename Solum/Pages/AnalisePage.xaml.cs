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
            NavigationPage.SetBackButtonTitle(this, "Voltar");
        }

        public AnalisePage(string analiseId)
        {
            InitializeComponent();
            BindingContext = new AnaliseViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, "Voltar");
        }

        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            if (Device.RuntimePlatform == "Android")
                boxSpacing.IsVisible = !boxSpacing.IsVisible;
        }

        private void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
           	if (Device.RuntimePlatform == "Android")
                boxSpacing.IsVisible = false;
        }
    }
}