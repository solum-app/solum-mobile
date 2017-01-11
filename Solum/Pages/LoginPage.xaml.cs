using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(Navigation);
            MessagingCenter.Subscribe<LoginViewModel, string>(this, "NullEntrys", (sender, args) =>{ MostarMensagem(args); }); // Mostra mensagem quando os campos estão vazios
            MessagingCenter.Subscribe<LoginViewModel, string>(this, "LoginError", (sender, args) =>{ MostarMensagem(args); }); // Mostra mensagem quando as credenciais estão inválidas
        }

        private async void MostarMensagem(string message)
        {
            await DisplayAlert("Erro", message, "Ok");
        }
    }
}