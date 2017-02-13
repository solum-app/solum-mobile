using System;
using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Settings;

namespace Solum.Pages
{
    public partial class TalhaoCadastroPage : ContentPage
    {
        public TalhaoCadastroPage(Fazenda fazenda, bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, fazenda, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }

        public TalhaoCadastroPage(Talhao talhao)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, talhao);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }

        private void NomeEntryOnCompleted(object sender, EventArgs e)
        {
            AreaEntry.Focus();
        }
    }
}