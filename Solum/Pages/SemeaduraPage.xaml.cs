using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class SemeaduraPage : ContentPage
    {
        public SemeaduraPage(string analiseId)
        {
            InitializeComponent();
            BindingContext = new SemeaduraViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}