using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class InterpretacaoPage : ContentPage
    {
        public InterpretacaoPage(Analise analise)
        {
            InitializeComponent();
            BindingContext = new InterpretacaoViewModel(Navigation, analise);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }


    }
}