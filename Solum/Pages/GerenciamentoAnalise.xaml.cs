using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class GerenciamentoAnalise : ContentPage
    {
        public GerenciamentoAnalise(Analise analise)
        {
            InitializeComponent();
            BindingContext = new GerenciamentoAnaliseViewModel(Navigation, analise);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}