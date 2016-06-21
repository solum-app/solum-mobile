using Xamarin.Forms;
using Solum.ViewModel;
using Solum.Models;

namespace Solum.Pages
{
	public partial class AnalisePage : ContentPage
	{
		public AnalisePage ()
		{
			NavigationPage.SetBackButtonTitle(this, "Voltar");

			InitializeComponent ();
			BindingContext = new AnaliseViewModel (Navigation);
		}

		public AnalisePage (Analise analise)
		{
			NavigationPage.SetBackButtonTitle (this, "Voltar");

			InitializeComponent ();
			BindingContext = new AnaliseViewModel (Navigation, analise);
		}
	}
}

