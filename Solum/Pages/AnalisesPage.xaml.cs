using System.Threading.Tasks;
using Solum.Renderers;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class AnalisesPage : ContentPage
    {
        public AnalisesPage()
        {
            InitializeComponent();
            BindingContext = new AnalisesViewModel(Navigation);
            NavigationPage.SetBackButtonTitle(this, "Voltar");

			if (Device.RuntimePlatform == "Android")
            {
				var fab = new FloatingActionButtonView
				{
					ImageName = "ic_add",
					ColorNormal = Color.FromHex("FFD54F"),
					ColorPressed = Color.FromHex("E6C047"),
					ColorRipple = Color.FromHex("FFD54F"),
					Clicked = async (arg1, arg2) => await ShowNewAnalisePage()
                };

                AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                absolute.Children.Add(fab);
            }
            else
            {
				var item = new ToolbarItem("Add", "ic_add", async () => await ShowNewAnalisePage());
                ToolbarItems.Add(item);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as AnalisesViewModel)?.UpdateAnalisesList();
        }

        private async Task ShowNewAnalisePage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new AnalisePage());
                IsBusy = false;
            }
        }
    }
}