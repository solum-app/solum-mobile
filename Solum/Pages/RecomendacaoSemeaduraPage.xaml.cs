using System.Linq;
using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class RecomendacaoSemeaduraPage : ContentPage
    {
		public RecomendacaoSemeaduraPage(string analiseId, int expectativaSelected, Cultura culturaSelected, bool allowEdit)
		{
			InitializeComponent();
			BindingContext = new RecomendacaoSemeaduraViewModel(Navigation, analiseId, expectativaSelected, culturaSelected, allowEdit);
			NavigationPage.SetBackButtonTitle(this, "Voltar");
			if (allowEdit)
			{
				ToolbarItems.Add(new ToolbarItem("Edit", "ic_editar", async () =>
				{
					if (!IsBusy)
					{
						IsBusy = true;
						await Navigation.PushAsync(new SemeaduraPage(analiseId));
						IsBusy = false;
					}
				}));
			}
        }
    }
}