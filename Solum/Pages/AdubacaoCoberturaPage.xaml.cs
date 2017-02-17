using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class AdubacaoCoberturaPage : ContentPage
    {
        public AdubacaoCoberturaPage(string analiseid, bool enableButton)
        {
            InitializeComponent();
            BindingContext = new AdubacaoCoberturaViewModel(Navigation, analiseid, enableButton);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}