using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        private string _email;
        private string _nome;

        public MenuViewModel(INavigation navigation) : base(navigation)
        {
            // var user = Realm.GetInstance().All<Usuario>().FirstOrDefault();
            Nome = "Teste";
            Email = "Teste@gmail.com";
        }

        public string Nome
        {
            get => _nome;
            set => SetPropertyChanged(ref _nome, value);
        }

        public string Email
        {
            get => _email;
            set => SetPropertyChanged(ref _email, value);
        }
    }
}