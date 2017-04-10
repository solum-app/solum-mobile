using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class FazendaCadastroPage : ContentPage
    {
        public FazendaCadastroPage(bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, "Voltar");
        }

        public FazendaCadastroPage(string fazendaId, bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation, fazendaId, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, "Voltar");
        }
    }
}