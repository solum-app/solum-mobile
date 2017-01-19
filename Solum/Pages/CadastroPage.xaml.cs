using Solum.ViewModel;
using Xamarin.Forms;
using static Solum.Messages.RegisterMessages;

namespace Solum.Pages
{
    public partial class CadastroPage : ContentPage
    {
        private static readonly string _buttonTitle = "OK";

        public CadastroPage()
        {
            InitializeComponent();
            BindingContext = new CadastroViewModel(Navigation);
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<CadastroViewModel, string>(this, EntryNullValuesTitle,
                async (view, arg) => { await DisplayAlert("Ops! :-/", EntryNullValuesMessage, _buttonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel, string>(this, RegisterSucessfullTitle,
                async (view, arg) => { await DisplayAlert("Tudo Certo! ;-)", RegisterSuccessfullMessage, _buttonTitle); });

            MessagingCenter.Subscribe<CadastroViewModel, string>(this, RegisterUnsuccessTitle,
                async (view, arg) => { await DisplayAlert("Ops! :-/", RegisterUnsuccessMessage, _buttonTitle); });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CadastroViewModel, string>(this, EntryNullValuesTitle);
            MessagingCenter.Unsubscribe<CadastroViewModel, string>(this, RegisterSucessfullTitle);
            MessagingCenter.Unsubscribe<CadastroViewModel, string>(this, RegisterUnsuccessTitle);
        }
    }
}