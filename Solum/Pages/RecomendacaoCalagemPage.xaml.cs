using System.Linq;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class RecomendacaoCalagemPage : ContentPage
    {
        public RecomendacaoCalagemPage(string analiseId, float v2ItemValue, float prnt, int profundidadeItemValue, bool allowEdit)
        {
            InitializeComponent();
            BindingContext = new RecomendacaoCalagemViewModel(Navigation, analiseId, v2ItemValue, prnt, profundidadeItemValue, allowEdit);
            NavigationPage.SetBackButtonTitle(this, "Voltar");

			if (allowEdit)
			{
				ToolbarItems.Add(new ToolbarItem("Edit", "ic_editar", async () =>
				{
					if (!IsBusy)
					{
						IsBusy = true;
						await Navigation.PushAsync(new CalagemPage(analiseId));
						IsBusy = false;
					}
				}));
			}
        }
    }
}