using System;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class CadastroPage : ContentPage
    {
        public CadastroPage()
        {
            InitializeComponent();
            BindingContext = new CadastroViewModel(Navigation);
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        private void NomeEntryOnCompleted(object sender, EventArgs e)
        {
            EmailEntry.Focus();
        }

        private void EmailEntryOnCompleted(object sender, EventArgs e)
        {
            SenhaEntry.Focus();
        }

        private void SenhaEntryOnCompleted(object sender, EventArgs e)
        {
            ConfirmarSenhaEntry.Focus();
        }
    }
}