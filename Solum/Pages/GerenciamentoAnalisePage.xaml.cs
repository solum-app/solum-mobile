using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class GerenciamentoAnalisePage : ContentPage
    {
        public GerenciamentoAnalisePage(string analiseId)
        {
            InitializeComponent();
            BindingContext = new GerenciamentoAnaliseViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as GerenciamentoAnaliseViewModel)?.UpdateValues();
        }
    }
}