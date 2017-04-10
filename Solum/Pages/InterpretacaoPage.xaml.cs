using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class InterpretacaoPage : ContentPage
    {
        public InterpretacaoPage(string analiseId)
        {
            InitializeComponent();
            BindingContext = new InterpretacaoViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, "Voltar");
        }


    }
}