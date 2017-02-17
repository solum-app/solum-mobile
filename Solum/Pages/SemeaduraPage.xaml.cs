using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class SemeaduraPage : ContentPage
    {
        public SemeaduraPage(string analiseId, bool enableButton)
        {
            InitializeComponent();
            BindingContext = new SemeaduraViewModel(Navigation, analiseId, enableButton);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}