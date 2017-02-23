using System.Linq;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class RecomendaSemeaduraPage : ContentPage
    {
        public RecomendaSemeaduraPage(string analiseId, int expectativaSelected, string culturaSelected, bool enableButton)
        {
            InitializeComponent();
            BindingContext = new RecomendacaoSemeaduraViewModel(Navigation, analiseId, expectativaSelected, culturaSelected, enableButton);
            NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
            ToolbarItems.Add(new ToolbarItem("Edit", "ic_editar", async () =>
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    //var current = Navigation.NavigationStack.LastOrDefault();
                    await Navigation.PushAsync(new SemeaduraPage(analiseId, true));
                    //Navigation.RemovePage(current);
                    IsBusy = false;
                }
            }));
        }
    }
}