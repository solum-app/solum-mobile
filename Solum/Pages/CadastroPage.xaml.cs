using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class CadastroPage : ContentPage
    {
        public CadastroPage()
        {
            BindingContext = new CadastroViewModel(Navigation);
            InitializeComponent();
        }
    }
}