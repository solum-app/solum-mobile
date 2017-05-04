using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum
{
	public partial class BemVindoPage : ContentPage
	{
		public BemVindoPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			NavigationPage.SetHasBackButton(this, false);
			BindingContext = new BemVindoViewModel(Navigation);
		}
	}
}