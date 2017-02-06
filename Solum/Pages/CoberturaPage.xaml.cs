using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class CoberturaPage : ContentPage
    {
        public CoberturaPage(string analiseid)
        {
            InitializeComponent();
            BindingContext = new CoberturaViewModel(Navigation, analiseid);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}