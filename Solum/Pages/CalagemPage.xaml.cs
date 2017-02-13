using System;
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

        private void V2Picker_OnSelectedIndexChanged(object sender, EventArgs e) => PrntEntry.Focus();

        private void PrntEntry_OnCompleted(object sender, EventArgs e) => ProfundidadePicker.Focus();
    }
}