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

			if (Device.RuntimePlatform == "iOS")
			{
				NomeEntry.HeightRequest = 40;
				EmailEntry.HeightRequest = 40;
				SenhaEntry.HeightRequest = 40;
				ConfirmarSenhaEntry.HeightRequest = 40;
				EstadosPicker.HeightRequest = 40;
				CidadesPicker.HeightRequest = 40;
			}
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