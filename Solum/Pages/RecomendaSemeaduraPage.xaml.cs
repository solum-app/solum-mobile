using System.Linq;
using Solum.Models;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class RecomendaSemeaduraPage : ContentPage
    {
        public RecomendaSemeaduraPage(string analiseId, int expectativaSelected, Cultura culturaSelected, bool enableButton)
        {
            InitializeComponent();
            BindingContext = new RecomendacaoSemeaduraViewModel(Navigation, analiseId, expectativaSelected, culturaSelected, enableButton);
            NavigationPage.SetBackButtonTitle(this, "Voltar");
            ToolbarItems.Add(new ToolbarItem("Edit", "ic_editar", async () =>
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    //var current = Navigation.NavigationStack.LastOrDefault();
                    await Navigation.PushAsync(new SemeaduraPage(analiseId));
                    //Navigation.RemovePage(current);
                    IsBusy = false;
                }
            }));
        }
    }
}