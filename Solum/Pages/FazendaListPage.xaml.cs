using System;
using Solum.Renderers;
using Solum.ViewModel;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class FazendaListPage : ContentPage
    {
        public FazendaListPage()
        {
            InitializeComponent();
            BindingContext = new FazendaListViewModel(Navigation);
            NavigationPage.SetBackButtonTitle(this, "Voltar");
            if (Device.OS == TargetPlatform.Android)
            {
                var fab = new FloatingActionButtonView
                {
                    ImageName = "ic_add",
                    ColorNormal = Color.FromHex("FFD54F"),
                    ColorPressed = Color.FromHex("E6C047"),
                    ColorRipple = Color.FromHex("FFD54F"),
                    Clicked = async (sender, args) =>
                        await Navigation.PushAsync(new FazendaCadastroPage())
                };
                AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(fab,
                    new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                absoluteLayout.Children.Add(fab);
            }
            else
            {
                var item = new ToolbarItem("Add", "ic_add", async () => await Navigation.PushAsync(new FazendaCadastroPage()));
                ToolbarItems.Add(item);
            }
        }


        private void OnEdit(object sender, EventArgs e)
        {
            var fazenda = (sender as MenuItem).CommandParameter;
            var context = BindingContext as FazendaListViewModel;
            context?.EditarCommand.Execute(fazenda);
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Confirmação", "Tem certeza que deseja excluir este item?", "Sim", "Não");
            if (!confirm) return;
            var fazenda = (sender as MenuItem).CommandParameter;
            var context = BindingContext as FazendaListViewModel;
            context?.ExcluirCommand.Execute(fazenda);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var context = BindingContext as FazendaListViewModel;
            context?.UpdateFazendaList();
        }
    }
}