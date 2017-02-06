using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
	public partial class CalagemPage : ContentPage
	{
		public CalagemPage(string analiseId)
		{
			InitializeComponent();
            BindingContext = new CalagemViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
		}
	}
}
