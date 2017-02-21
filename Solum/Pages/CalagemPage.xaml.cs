using System;
using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class CalagemPage : ContentPage
    {
        public CalagemPage(string analiseId)
        {
            InitializeComponent();
            BindingContext = new CalagemViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }

        private void PrntEntry_OnFocused(object sender, FocusEventArgs e)
        {
            var text = ((Entry) sender).Text;
            if(!string.IsNullOrEmpty(text))
                ((Entry) sender).Text = text.Replace("%", "").Trim();
        }

        private void PrntEntry_OnUnfocused(object sender, FocusEventArgs e)
        {
            if(!string.IsNullOrEmpty(((Entry)sender).Text))
                ((Entry)sender).Text += " %";
        }
    }
}