using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class FazendaCadastroPage : ContentPage
    {
        public FazendaCadastroPage()
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation);
            MessagingCenter.Subscribe<FazendaCadastroViewModel, string>(this, "Erro", (sender, args) => ShowErrorMessage(args));
            MessagingCenter.Subscribe<FazendaCadastroViewModel, string>(this, "Sucesso", (sender, args) => ShowSuccessMessage(args));
        }

        public FazendaCadastroPage(Fazenda fazenda)
        {
            InitializeComponent();
            BindingContext = new FazendaCadastroViewModel(Navigation, fazenda);
            MessagingCenter.Subscribe<FazendaCadastroViewModel, string>(this, "Erro", (sender, args) => ShowErrorMessage(args));
            MessagingCenter.Subscribe<FazendaCadastroViewModel, string>(this, "Sucesso", (sender, args) => ShowSuccessMessage(args));
        }

        private async void ShowErrorMessage(string message) => await DisplayAlert("Erro", message, "Ok");
        private async void ShowSuccessMessage(string message) => await DisplayAlert("Sucesso", message, "Ok");
    }
}