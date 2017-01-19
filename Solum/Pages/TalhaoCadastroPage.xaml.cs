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

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "NullEntrys",
                (sender, args) => ShowErrorMessage(args));

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "Success",
                (sender, args) => ShowSuccessMessage(args));
        }

        public TalhaoCadastroPage(Fazenda fazenda, Talhao talhao)
        {
            InitializeComponent();
            BindingContext = new TalhaoCadastroViewModel(Navigation, fazenda, talhao);

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "NullEntrys",
                (sender, args) => ShowErrorMessage(args));

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "Success",
                (sender, args) => ShowSuccessMessage(args));
        }

        private async void ShowErrorMessage(string message)
        {
            await DisplayAlert("Erro!", message, "OK");
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel, string>(this, "NullEntrys");
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel, string>(this, "Success");

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "NullEntrys",
                (sender, args) => ShowErrorMessage(args));

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "Success",
                (sender, args) => ShowSuccessMessage(args));
        }

        private async void ShowSuccessMessage(string message)
        {
            await DisplayAlert("Sucesso!", message, "OK");
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel, string>(this, "NullEntrys");
            MessagingCenter.Unsubscribe<TalhaoCadastroViewModel, string>(this, "Success");

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "NullEntrys",
                (sender, args) => ShowErrorMessage(args));

            MessagingCenter.Subscribe<TalhaoCadastroViewModel, string>(this, "Success",
                (sender, args) => ShowSuccessMessage(args));
        }
    }
}