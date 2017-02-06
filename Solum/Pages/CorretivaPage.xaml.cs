using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class CorretivaPage : ContentPage
    {
        public CorretivaPage(string analiseId)
        {
            InitializeComponent();
            BindingContext = new CorretivaViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}