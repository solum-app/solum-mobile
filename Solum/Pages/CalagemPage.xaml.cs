using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
	public partial class CalagemPage : ContentPage
	{
		public CalagemPage(INavigation navigation, string analiseId)
		{
			InitializeComponent();
            BindingContext = new CalagemViewModel(navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
		}
	}
}
