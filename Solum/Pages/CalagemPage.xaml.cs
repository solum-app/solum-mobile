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

        public CalagemPage(Analise analise)
        {
            InitializeComponent();
            BindingContext = new CalagemViewModel(Navigation, analise);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }

        private void PrntEntry_OnCompleted(object sender, EventArgs e)
        {
            var entry = sender as Entry;
            var text = entry.Text;
            if (!text.Contains("%"))
                entry.Text = text + " %";
        }
    }
}