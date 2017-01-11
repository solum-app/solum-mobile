using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        private string _name, _username;

        public MenuViewModel(INavigation navigation) : base(navigation)
        {
            var uds = new UserDataService();
            var user = uds.GetLoggedUser();
            Name = user.Nome;
            Username = user.Username;
        }
        public string Name
        {
            get { return _name; }
            set { SetPropertyChanged(ref _name, value); }
        }

        public string Username
        {
            get { return _username; }
            set { SetPropertyChanged(ref _username, value); }
        }
    }
}