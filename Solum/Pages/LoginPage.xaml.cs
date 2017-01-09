using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            BindingContext = new LoginViewModel(Navigation);
            InitializeComponent();
        }
    }
}