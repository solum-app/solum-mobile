using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class AdubacaoCoberturaPage : ContentPage
    {
        public AdubacaoCoberturaPage(string analiseid)
        {
            InitializeComponent();
            BindingContext = new AdubacaoCoberturaViewModel(Navigation, analiseid);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}