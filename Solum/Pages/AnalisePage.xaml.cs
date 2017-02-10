using System;
using Solum.Models;
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

        public AnalisePage(Analise analise)
        {
            InitializeComponent();
            BindingContext = new AnaliseViewModel(Navigation, analise);
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

        private void AnaliseNameEntry_OnCompleted(object sender, EventArgs e) => FazendaLabel.Focus();

        private void PhEntry_OnCompleted(object sender, EventArgs e) => FosforoEntry.Focus();

        private void FosforoEntry_OnCompleted(object sender, EventArgs e) => PotassioEntry.Focus();

        private void PotassioEntry_OnCompleted(object sender, EventArgs e) => CalcioEntry.Focus();

        private void CalcioEntry_OnCompleted(object sender, EventArgs e) => MagnesioEntry.Focus();

        private void MagnesioEntry_OnCompleted(object sender, EventArgs e) => AluminioEntry.Focus();

        private void AluminioEntry_OnCompleted(object sender, EventArgs e) => HidrogenioEntry.Focus();

        private void HidrogenioEntry_OnCompleted(object sender, EventArgs e) => MateriaOrganicaEntry.Focus();

        private void MateriaOrganicaEntry_OnCompleted(object sender, EventArgs e) => AreiaEntry.Focus();

        private void AreiaEntry_OnCompleted(object sender, EventArgs e) => SilteEntry.Focus();

        private void SilteEntry_OnCompleted(object sender, EventArgs e) => ArgilaEntry.Focus();

        private void ArgilaEntry_OnCompleted(object sender, EventArgs e)
        {
            ArgilaEntry.Unfocus();
        }
    }
}