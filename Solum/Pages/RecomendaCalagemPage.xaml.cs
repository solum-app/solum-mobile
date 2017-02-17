using System.Linq;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class RecomendaCalagemPage : ContentPage
    {
        public RecomendaCalagemPage(string analiseId, float v2ItemValue, float prnt, float profundidadeItemValue, bool enableButton)
        {
            InitializeComponent();
            BindingContext = new RecomendacaoCalagemViewModel(Navigation, analiseId, v2ItemValue, prnt, profundidadeItemValue, enableButton);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
            ToolbarItems.Add(new ToolbarItem("Edit", "ic_editar", async () =>
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    var current = Navigation.NavigationStack.LastOrDefault();
                    await Navigation.PushAsync(new CalagemPage(analiseId));
                    Navigation.RemovePage(current);
                    IsBusy = false;
                }
            }));
        }
    }
}