using System;
using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Settings;

namespace Solum.Pages
{
    public partial class TalhaoCadastroPage : ContentPage
    {
        public TalhaoCadastroPage(string fazendaId, bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, fazendaId, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }

        public TalhaoCadastroPage(string talhaoId)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, talhaoId);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }

        private void NomeEntryOnCompleted(object sender, EventArgs e)
        {
            AreaEntry.Focus();
        }
    }
}