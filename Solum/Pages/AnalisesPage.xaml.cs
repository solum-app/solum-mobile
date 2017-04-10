using System;
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

            if (Device.OS == TargetPlatform.Android)
            {
                var fab = new FloatingActionButtonView
                {
                    ImageName = "ic_add",
                    ColorNormal = Color.FromHex("FFD54F"),
                    ColorPressed = Color.FromHex("E6C047"),
                    ColorRipple = Color.FromHex("FFD54F"),
                    Clicked = (sender, args) => ShowNewAnalisePage()
                };

                AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                absolute.Children.Add(fab);
            }
            else
            {
                var item = new ToolbarItem("Add", "ic_add", ShowNewAnalisePage);
                ToolbarItems.Add(item);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as AnalisesViewModel)?.UpdateAnalisesList();
        }

        private async void ShowNewAnalisePage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new AnalisePage());
                IsBusy = false;
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
            var analise = (sender as MenuItem)?.CommandParameter;
            (BindingContext as AnalisesViewModel)?.EditCommand.Execute(analise);
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Confirmação", "Tem certeza que deseja excluir este item?\nTodos os dados relacionados à essa análise serão exlcuidos!", "Sim", "Não");
            if (confirm)
            {
                var analise = (sender as MenuItem)?.CommandParameter;
                (BindingContext as AnalisesViewModel)?.DeleteCommand.Execute(analise);
            }
        }
    }
}