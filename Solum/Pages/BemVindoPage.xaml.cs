using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum
{
	public partial class BemVindoPage : ContentPage
	{
		public BemVindoPage()
		{
			InitializeComponent();
			BindingContext = new BemVindoViewModel(Navigation);
		}
	}
}
