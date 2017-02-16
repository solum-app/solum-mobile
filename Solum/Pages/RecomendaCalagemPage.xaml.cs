using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
	public partial class RecomendaCalagemPage : ContentPage
	{
		public RecomendaCalagemPage(string analiseId)
		{
			InitializeComponent();
            BindingContext = new RecomendacaoCalagemViewModel(Navigation, analiseId);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
		}

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
            (BindingContext as RecomendacaoCalagemViewModel)?.Calculate();
	    }
	}
}
