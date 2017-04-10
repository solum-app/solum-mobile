using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class AdubacaoCorretivaPage : ContentPage
    {
        public AdubacaoCorretivaPage(string analiseId, bool enableButton)
        {
            InitializeComponent();
            BindingContext = new AdubacaoCorretivaViewModel(Navigation, analiseId, enableButton);
            NavigationPage.SetBackButtonTitle(this, "Voltar");
        }
    }
}