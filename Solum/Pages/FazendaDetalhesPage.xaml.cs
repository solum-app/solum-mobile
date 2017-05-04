using System;
using Solum.Handlers;
using Solum.Models;
using Solum.Renderers;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class FazendaDetalhesPage : ContentPage
    {
        public FazendaDetalhesPage(string fazendaId, bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new FazendaDetalhesViewModel(Navigation, fazendaId, fromAnalise);
            NavigationPage.SetBackButtonTitle(this, "Voltar");

			if (Device.RuntimePlatform == "Android")
            {
                var fab = new FloatingActionButtonView
                {
                    ImageName = "ic_add",
                    ColorNormal = Color.FromHex("FFD54F"),
                    ColorPressed = Color.FromHex("E6C047"),
                    ColorRipple = Color.FromHex("FFD54F"),
                    Clicked = async (sender, args) =>
                    {
                        if (!IsBusy)
                        {
                            IsBusy = true;
                            await Navigation.PushAsync(new TalhaoCadastroPage(fazendaId, fromAnalise));
                            if (fromAnalise)
                                Navigation.RemovePage(this);
                            IsBusy = false;
                        }
                    }
                };

                AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(fab,
                    new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                absoluteLayout.Children.Add(fab);
            }
            else
            {
                var item = new ToolbarItem("Add", "ic_add",
                    async () =>
                    {
                        if (!IsBusy)
                        {
                            IsBusy = true;
                            await Navigation.PushAsync(new TalhaoCadastroPage(fazendaId, fromAnalise));
                            if (fromAnalise)
                                Navigation.RemovePage(this);
                            IsBusy = false;
                        }
                    });
                ToolbarItems.Add(item);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as FazendaDetalhesViewModel)?.UpdateTalhoesList();
        }
    }
}