using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class TalhaoCadastroPage : ContentPage
    {
        public TalhaoCadastroPage(Fazenda fazenda)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, fazenda);
			NavigationPage.SetBackButtonTitle(this, "Voltar");
			
        }

        public TalhaoCadastroPage(Talhao talhao)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, talhao);
			NavigationPage.SetBackButtonTitle(this, "Voltar");
			
        }

        private async void ShowErrorMessage(string message)
        {
            await DisplayAlert("Erro!", message, "OK");
        }

        private async void ShowSuccessMessage(string message)
        {
            await DisplayAlert("Sucesso!", message, "OK");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "NullEntrys", (sender, args) => ShowErrorMessage(args));
            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "Success", (sender, args) => ShowSuccessMessage(args));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel, string>(this, "NullEntrys");
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel, string>(this, "Success");
        }
    }
}