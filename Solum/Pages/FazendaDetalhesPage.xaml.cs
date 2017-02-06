using System;
using Solum.Models;
using Solum.Renderers;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class FazendaDetalhesPage : ContentPage
    {
        public FazendaDetalhesPage(Fazenda fazenda, bool fromAnalise)
        {
            InitializeComponent();
            BindingContext = new FazendaDetalhesViewModel(Navigation, fazenda, fromAnalise);
			NavigationPage.SetBackButtonTitle(this, Settings.BackButtonTitle);
			
            if (Device.OS == TargetPlatform.Android)
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
                            await Navigation.PushAsync(new TalhaoCadastroPage(fazenda, fromAnalise));
                            if(fromAnalise)
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
                    async () => {
                        if (!IsBusy)
                        {
                            IsBusy = true;
                            await Navigation.PushAsync(new TalhaoCadastroPage(fazenda, fromAnalise));
                            if (fromAnalise)
                                Navigation.RemovePage(this);
                            IsBusy = false;
                        }
                    });
                ToolbarItems.Add(item);
            }
        }


        private void OnEdit(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var fazenda = menuItem.CommandParameter;
                var context = BindingContext as FazendaDetalhesViewModel;
                context?.ShowEditTalhaoPageCommand.Execute(fazenda);
            }
        }

        public async void OnDelete(object sender, EventArgs args)
        {
            var confirm = await DisplayAlert("Confirmação", "Tem certeza que deseja excluir este item?", "Sim", "Não");
            if (!confirm) return;
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var talhao = menuItem.CommandParameter;
                var context = BindingContext as FazendaDetalhesViewModel;
                context?.DeleteTalhaoCommand.Execute(talhao);
            }
        }

        protected override void OnAppearing()
        {
            var context = BindingContext as FazendaDetalhesViewModel;
            context?.UpdateTalhoesList();
            base.OnAppearing();
            
        }
    }
}