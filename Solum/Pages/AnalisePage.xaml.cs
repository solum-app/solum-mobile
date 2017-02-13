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

        private void PhEntryOnCompleted(object sender, EventArgs e)
        {
            FosforoEntry.Focus();
        }

        private void FosforoEntryOnCompleted(object sender, EventArgs e)
        {
            PotassioEntry.Focus();
        }

        private void PotassioEntryOnCompleted(object sender, EventArgs e)
        {
            CalcioEntry.Focus();
        }

        private void CalcioEntryOnCompleted(object sender, EventArgs e)
        {
            MagnesioEntry.Focus();
        }

        private void MagnesioEntryOnCompleted(object sender, EventArgs e)
        {
            AluminioEntry.Focus();
        }

        private void AluminioEntryOnCompleted(object sender, EventArgs e)
        {
            HidrogenioEntry.Focus();
        }

        private void HidrogenioEntryOnCompleted(object sender, EventArgs e)
        {
            MateriaOrganicaEntry.Focus();
        }

        private void MateriaOrganicaEntryOnCompleted(object sender, EventArgs e)
        {
            AreiaEntry.Focus();
        }

        private void AreiaEntryOnCompleted(object sender, EventArgs e)
        {
            SilteEntry.Focus();
        }

        private void SilteEntryOnCompleted(object sender, EventArgs e)
        {
            ArgilaEntry.Focus();
        }

        private void ArgilaEntryOnCompleted(object sender, EventArgs e)
        {
            ArgilaEntry.Unfocus();
        }
    }
}