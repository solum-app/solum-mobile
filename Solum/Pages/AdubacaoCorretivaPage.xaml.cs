using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class AdubacaoCorretivaPage : ContentPage
    {
        public AdubacaoCorretivaPage(string analiseId)
        {
            InitializeComponent();
            BindingContext = new AdubacaoCorretivaViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
        }
    }
}