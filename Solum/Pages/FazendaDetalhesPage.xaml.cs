using System;
using Solum.Models;
using Solum.Renderers;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class FazendaDetalhesPage : ContentPage
    {
        public FazendaDetalhesPage(Fazenda fazenda)
        {
            InitializeComponent();
            BindingContext = new FazendaDetalhesViewModel(Navigation, fazenda);
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
                            await Navigation.PushAsync(new TalhaoCadastroPage(fazenda));
                            IsBusy = false;
                        }
                    }
                };

                // Overlay the FAB in the bottom-right corner
                AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(fab,
                    new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                absoluteLayout.Children.Add(fab);
            }
            else
            {
                var item = new ToolbarItem("Add", "ic_add",
                    async () => {
                        IsBusy = true;
                        await Navigation.PushAsync(new TalhaoCadastroPage(fazenda));
                        IsBusy = false;
                    });
                ToolbarItems.Add(item);
            }
        }


        private void OnEdit(object sender, EventArgs e)
        {
            var fazenda = (sender as MenuItem).CommandParameter;
            var context = BindingContext as FazendaDetalhesViewModel;
            context?.EditarTalhaoCommand.Execute(fazenda);
        }

        public async void OnDelete(object sender, EventArgs args)
        {
            var confirm = await DisplayAlert("Confirmação", "Tem certeza que deseja excluir este item?", "Sim", "Não");
            if (!confirm) return;
            var talhao = (sender as MenuItem).CommandParameter;
            var context = BindingContext as FazendaDetalhesViewModel;
            context?.RemoverTalhaoCommand.Execute(talhao);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var context = BindingContext as FazendaDetalhesViewModel;
            context?.UpdateTalhoesList();
        }
    }
}