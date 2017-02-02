using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class CorretivaPage : ContentPage
    {
        public CorretivaPage(INavigation navigation, string analiseId)
        {
            InitializeComponent();
            BindingContext = new CorretivaViewModel(navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}