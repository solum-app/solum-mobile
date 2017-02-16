using System;
using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Settings;

namespace Solum.Pages
{
    public partial class FazendaCadastroPage : ContentPage
    {
        public FazendaCadastroPage(bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }

        public FazendaCadastroPage(string fazendaId, bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation, fazendaId, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, BackButtonTitle);
        }
    }
}